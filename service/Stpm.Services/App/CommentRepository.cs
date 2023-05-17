using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.Comment;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using Stpm.Services.Extensions;

namespace Stpm.Services.App;

public class CommentRepository : ICommentRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public CommentRepository(StpmDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<IList<Comment>> GetCommentsAsync(CancellationToken cancellationToken = default)
    {
        var commentQuery = _dbContext.Comments.AsSplitQuery().AsNoTracking();

        return await commentQuery.ToListAsync(cancellationToken);
    }

    public async Task<Comment> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Comments.AsSplitQuery()
                                        .Where(t => t.Id == id)
                                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Comment> GetCachedCommentByIdAsync(int commentId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"comment.by-id.{commentId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetCommentByIdAsync(commentId, cancellationToken);
            });
    }

    public async Task<IPagedList<Comment>> GetCommentByQueryAsync(CommentQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterComments(query).ToPagedListAsync(
                                pageNumber,
                                pageSize,
                                nameof(CommentQuery.UserId),
                                "DESC",
                                cancellationToken);
    }

    public async Task<IPagedList<Comment>> GetCommentByQueryAsync(CommentQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await FilterComments(query).ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetCommentByQueryAsync<T>(CommentQuery query, IPagingParams pagingParams, Func<IQueryable<Comment>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        IQueryable<T> result = mapper(FilterComments(query));

        return await result.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> AddOrUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        if (comment.Id > 0)
        {
            _dbContext.Update(comment);
        }
        else
        {
            await _dbContext.AddAsync(comment, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteCommentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var comment = await _dbContext.Comments.FindAsync(id);

        if (comment is null) return false;

        _dbContext.Comments.Remove(comment);
        var rowsCount = await _dbContext.SaveChangesAsync(cancellationToken);

        return rowsCount > 0;
    }

    private IQueryable<Comment> FilterComments(CommentQuery query)
    {
        IQueryable<Comment> commentQuery = _dbContext.Comments.AsSplitQuery()
                                                              .AsNoTracking();

        if (query?.UserId > 0)
        {
            commentQuery = commentQuery.Where(x => x.User.Id == query.UserId);
        }

        if (query?.PostId > 0)
        {
            commentQuery = commentQuery.Where(x => x.Posts.Any(p => p.Id == query.PostId));
        }

        if (query?.TopicId > 0)
        {
            commentQuery = commentQuery.Where(x => x.Topics.Any(t => t.Id == query.TopicId));
        }

        if (!string.IsNullOrWhiteSpace(query.TopicSlug))
        {
            commentQuery = commentQuery.Where(x => x.Topics.Any(t => t.UrlSlug == query.TopicSlug));
        }

        if (!string.IsNullOrWhiteSpace(query.UserSlug))
        {
            commentQuery = commentQuery.Where(x => x.User.UrlSlug == query.UserSlug);
        }

        if (!string.IsNullOrWhiteSpace(query.PostSlug))
        {
            commentQuery = commentQuery.Where(x => x.Posts.Any(p => p.UrlSlug == query.UserSlug));
        }

        if (query?.Year > 0)
        {
            commentQuery = commentQuery.Where(x => x.Date.Year == query.Year);
        }

        if (query?.Month > 0)
        {
            commentQuery = commentQuery.Where(x => x.Date.Month == query.Month);
        }

        if (query?.Day > 0)
        {
            commentQuery = commentQuery.Where(x => x.Date.Day == query.Day);
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            commentQuery = commentQuery.Where(x => x.Content.Contains(query.Keyword) ||
                                             x.Posts.Any(p => p.Title.Contains(query.Keyword)) ||
                                             x.Topics.Any(t => t.TopicName.Contains(query.Keyword)) ||
                                             x.User.FullName.Contains(query.Keyword));
        }

        return commentQuery;
    }
}
