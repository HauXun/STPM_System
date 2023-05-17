using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.Post;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using Stpm.Services.Extensions;

namespace Stpm.Services.App;

public class PostRepository : IPostRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly ITagRepository _tagRepository;
    private readonly IMemoryCache _memoryCache;

    public PostRepository(StpmDbContext dbContext, ITagRepository tagRepository, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _tagRepository = tagRepository;
        _memoryCache = memoryCache;
    }

    public async Task<IList<Post>> GetPostsAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postsQuery = _dbContext.Posts.Include(p => p.Tags)
                                                      .Include(p => p.PostPhotos)
                                                      .Include(p => p.PostVideos)
                                                      .Include(p => p.User)
                                                      .AsSplitQuery()
                                                      .AsNoTracking();

        return await postsQuery.ToListAsync(cancellationToken);
    }

    public async Task<Post> GetPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postQuery = _dbContext.Posts.Include(p => p.Tags)
                                                     .Include(p => p.PostPhotos)
                                                     .Include(p => p.PostVideos)
                                                     .Include(p => p.User)
                                                     .AsSplitQuery()
                                                     .Where(p => p.Id.Equals(id));

        if (published)
        {
            postQuery = postQuery.Where(x => x.Published);
        }

        return await postQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Post> GetCachedPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"post.by-id.{id}-{published}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetPostByIdAsync(id, published, cancellationToken);
            });
    }

    public async Task<Post> GetPostBySlugAsync(string slug, bool published = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postQuery = _dbContext.Posts.Include(p => p.Tags)
                                                     .Include(p => p.PostPhotos)
                                                     .Include(p => p.PostVideos)
                                                     .Include(p => p.User)
                                                     .AsSplitQuery()
                                                     .Where(p => p.UrlSlug.Equals(slug));

        if (published)
        {
            postQuery = postQuery.Where(x => x.Published);
        }

        return await postQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Post> GetCachedPostBySlugAsync(string slug, bool published = false, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"post.by-slug.{slug}-{published}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetPostBySlugAsync(slug, published, cancellationToken);
            });
    }

    public async Task<IList<Post>> GetPopularPostsAsync(int limit, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Posts.Include(p => p.Tags)
                                    .Include(p => p.PostPhotos)
                                    .Include(p => p.PostVideos)
                                    .Include(p => p.User)
                                    .AsSplitQuery()
                                    .AsNoTracking()
                                    .OrderByDescending(p => p.ViewCount)
                                    .Take(limit)
                                    .ToListAsync(cancellationToken);
    }

    public async Task<IList<Post>> GetRandomPostAsync(int limit, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Posts.Include(p => p.Tags)
                                    .Include(p => p.PostPhotos)
                                    .Include(p => p.PostVideos)
                                    .Include(p => p.User)
                                    .AsNoTracking()
                                    .OrderBy(p => Guid.NewGuid())
                                    .Take(limit)
                                    .ToListAsync(cancellationToken);
    }

    public async Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterPosts(query).ToPagedListAsync(
                                pageNumber,
                                pageSize,
                                nameof(Post.PostedDate),
                                "DESC",
                                cancellationToken);
    }

    public async Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await FilterPosts(query).ToPagedListAsync(
                                        pagingParams,
                                        cancellationToken);
    }

    public async Task<IPagedList<T>> GetPostByQueryAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        IQueryable<T> result = mapper(FilterPosts(query));

        return await result.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        if (post.Id > 0)
        {
            await _dbContext.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
        }
        else
        {
            post.Tags = new List<Tag>();
        }

        var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
          .Select(x => new
          {
              Name = x,
              Slug = x.GenerateSlug()
          })
          .GroupBy(x => x.Slug)
          .ToDictionary(g => g.Key, g => g.First().Name);

        foreach (var kv in validTags)
        {
            if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

            var tag = await _tagRepository.GetTagBySlugAsync(kv.Key, cancellationToken) ?? new Tag()
            {
                Name = kv.Value,
                Description = kv.Value,
                UrlSlug = kv.Key
            };

            post.Tags.Add(tag);
        }

        post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

        if (post.Id > 0)
            _dbContext.Update(post);
        else
            await _dbContext.AddAsync(post, cancellationToken);

        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var post = await _dbContext.Posts.FindAsync(id);

        if (post is null) return false;

        _dbContext.Posts.Remove(post);
        var rowsCount = await _dbContext.SaveChangesAsync(cancellationToken);

        return rowsCount > 0;
    }

    public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Posts.AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
    }

    public async Task<bool> IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Posts
                               .Where(x => x.Id == postId)
                               .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken) > 0;
    }

    public async Task<bool> ChangePostStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var post = await _dbContext.Posts.FindAsync(id);

        if (post == null) return false;

        post.Published = !post.Published;

        _dbContext.Attach(post).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddImageUrlAsync(int postId, string imageUrl, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(new PostPhoto
        {
            ImageUrl = imageUrl,
            PostId = postId
        }, cancellationToken);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddVideoUrlAsync(int postId, string videoUrl, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(new PostVideo
        {
            VideoUrl = videoUrl,
            PostId = postId
        }, cancellationToken);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveImageUrlAsync(int postId, string imageUrl, CancellationToken cancellationToken = default)
    {
        var postPhoto = await _dbContext.PostPhotos.Where(t => t.PostId == postId && t.ImageUrl == imageUrl).FirstOrDefaultAsync(cancellationToken);

        if (postPhoto == null) return false;

        _dbContext.PostPhotos.Remove(postPhoto);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveVideoUrlAsync(int postId, string videoUrl, CancellationToken cancellationToken = default)
    {
        var postVideo = await _dbContext.PostVideos.Where(t => t.PostId == postId && t.VideoUrl == videoUrl).FirstOrDefaultAsync(cancellationToken);

        if (postVideo == null) return false;

        _dbContext.PostVideos.Remove(postVideo);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private IQueryable<Post> FilterPosts(PostQuery query)
    {
        IQueryable<Post> postsQuery = _dbContext.Posts.Include(p => p.Tags)
                                                      .Include(p => p.PostPhotos)
                                                      .Include(p => p.PostVideos)
                                                      .Include(p => p.User)
                                                      .AsSplitQuery()
                                                      .AsNoTracking();

        if (query.Published != null)
        {
            postsQuery = postsQuery.Where(x => x.Published == query.Published);
        }

        if (query?.UserId > 0)
        {
            postsQuery = postsQuery.Where(x => x.UserId == query.UserId);
        }

        if (!string.IsNullOrWhiteSpace(query.UserSlug))
        {
            postsQuery = postsQuery.Where(x => x.User.UrlSlug == query.UserSlug);
        }

        if (!string.IsNullOrWhiteSpace(query.TagSlug))
        {
            postsQuery = postsQuery.Where(x => x.Tags.Any(t => t.UrlSlug == query.TagSlug));
        }

        if (!string.IsNullOrWhiteSpace(query.PostSlug))
        {
            postsQuery = postsQuery.Where(x => x.UrlSlug == query.PostSlug);
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            postsQuery = postsQuery.Where(x => x.Title.Contains(query.Keyword) ||
                                             x.ShortDescription.Contains(query.Keyword) ||
                                             x.Description.Contains(query.Keyword) ||
                                             x.User.FullName.Contains(query.Keyword) ||
                                             x.Tags.Any(t => t.Name.Contains(query.Keyword)));
        }

        if (query.Year > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Year == query.Year);
        }

        if (query.Month > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Month == query.Month);
        }

        if (query.Day > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Day == query.Day);
        }

        query.GetTagListAsync();
        if (query.SelectedTag != null && query.SelectedTag.Count() > 0)
        {
            postsQuery = postsQuery.Where(p => query.SelectedTag.Intersect(p.Tags.Select(t => t.Name)).Count() > 0);
        }

        return postsQuery;
    }
}
