using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.Topic;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using Stpm.Services.Extensions;

namespace Stpm.Services.App;

public class TopicRepository : ITopicRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;
    private readonly IUserRepository _userRepository;

    public TopicRepository(StpmDbContext dbContext, IMemoryCache memoryCache, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
        _userRepository = userRepository;
    }

    public async Task<IList<Topic>> GetTopicsAsync(CancellationToken cancellationToken = default)
    {
        var topicQuery = _dbContext.Topics.Include(t => t.SpecificAward)
                                          .Include(t => t.Leader)
                                          .Include(t => t.UserTopicRatings)
                                          .Include(t => t.Users)
                                          .Include(t => t.TopicRank)
                                          .Include(t => t.TopicPhotos)
                                          .Include(t => t.TopicVideos)
                                          .AsSplitQuery()
                                          .AsNoTracking();

        return await topicQuery.ToListAsync(cancellationToken);
    }

    public async Task<Topic> GetTopicByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Topics.Include(t => t.SpecificAward)
                                      .Include(t => t.Leader)
                                      .Include(t => t.UserTopicRatings)
                                      .Include(t => t.Users)
                                      .Include(t => t.Comments)
                                      .Include(t => t.TopicRank)
                                      .Include(t => t.TopicPhotos)
                                      .Include(t => t.TopicVideos)
                                      .AsSplitQuery()
                                      .Where(t => t.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Topic> GetCachedTopicByIdAsync(int topicId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"topic.by-id.{topicId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetTopicByIdAsync(topicId, cancellationToken);
            });
    }

    public async Task<Topic> GetTopicBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _dbContext.Topics.Include(t => t.SpecificAward)
                                      .Include(t => t.Leader)
                                      .Include(t => t.UserTopicRatings)
                                      .Include(t => t.Users)
                                      .Include(t => t.Comments)
                                      .Include(t => t.TopicRank)
                                      .Include(t => t.TopicPhotos)
                                      .Include(t => t.TopicVideos)
                                      .AsSplitQuery()
                                      .Where(t => t.UrlSlug.Equals(slug)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Topic> GetCachedTopicBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"topic.by-slug.{slug}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetTopicBySlugAsync(slug, cancellationToken);
            });
    }

    public async Task<IPagedList<Topic>> GetTopicByQueryAsync(TopicQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterTopics(query).ToPagedListAsync(
                                pageNumber,
                                pageSize,
                                nameof(TopicQuery.TopicName),
                                "DESC",
                                cancellationToken);
    }

    public async Task<IPagedList<Topic>> GetTopicByQueryAsync(TopicQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await FilterTopics(query).ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetTopicByQueryAsync<T>(TopicQuery query, IPagingParams pagingParams, Func<IQueryable<Topic>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        IQueryable<T> result = mapper(FilterTopics(query));

        return await result.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> AddOrUpdateTopicAsync(Topic topic, CancellationToken cancellationToken = default)
    {
        if (topic.Id > 0)
        {
            _dbContext.Update(topic);
        }
        else
        {
            await _dbContext.AddAsync(topic, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddOrUpdateTopicHistoryAsync(TopicHistoryAward topic, CancellationToken cancellationToken = default)
    {
        if (topic.Id > 0)
        {
            _dbContext.Update(topic);
        }
        else
        {
            await _dbContext.AddAsync(topic, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> SwitchRegisteredStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var topic = await _dbContext.Topics.FindAsync(id);

        if (topic == null) return false;

        topic.Registered = true;
        topic.Cancel = !topic.Registered;
        topic.ForceLock = !topic.Registered;

        _dbContext.Attach(topic).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> SwitchCancelStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var topic = await _dbContext.Topics.FindAsync(id);

        if (topic == null) return false;

        topic.Cancel = true;
        topic.Registered = !topic.Cancel;
        topic.ForceLock = !topic.Cancel;

        if (topic.CancelDate != null)
            topic.CancelDate = null;
        else
            topic.CancelDate = DateTime.UtcNow;

        _dbContext.Attach(topic).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> SwitchForceLockStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var topic = await _dbContext.Topics.FindAsync(id);

        if (topic == null) return false;

        topic.ForceLock = true;
        topic.Registered = !topic.ForceLock;
        topic.Cancel = !topic.ForceLock;

        if (topic.CancelDate != null)
            topic.CancelDate = null;
        else
            topic.CancelDate = DateTime.UtcNow;

        _dbContext.Attach(topic).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> SpecificTopicUserAsync(int userId, int topicId, CancellationToken cancellationToken = default)
    {
        var topic = await _dbContext.Topics.FindAsync(topicId);
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (topic == null || user == null) return false;

        if (topic.Users.Any(u => u.Id == user.Id))
            return true;

        topic.Users.Add(user);

        _dbContext.Attach(topic).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddOrUpdateUserSpecificMarkAsync(int userId, int topicId, float? mark = null, CancellationToken cancellationToken = default)
    {
        if (mark != null)
        {
            return await _dbContext.UserTopicRatings.Where(u => u.UserId == userId && u.TopicId == topicId).ExecuteUpdateAsync(x => x.SetProperty(a => a.Mark, mark), cancellationToken) > 0;
        }
        else
        {
            await _dbContext.AddAsync(new UserTopicRating
            {
                UserId = userId,
                TopicId = topicId
            }, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UserRemoveSpecificMarkAsync(int userId, int topicId, CancellationToken cancellationToken = default)
    {
        var userTopicRating = await _dbContext.UserTopicRatings.Where(t => t.TopicId == topicId && t.UserId == userId).FirstOrDefaultAsync(cancellationToken);

        if (userTopicRating == null) return false;

        _dbContext.UserTopicRatings.Remove(userTopicRating);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveSpecificTopicUserAsync(int userId, int topicId, CancellationToken cancellationToken = default)
    {
        var topic = await _dbContext.Topics.FindAsync(topicId);
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (topic == null || user == null) return false;

        if (!topic.Users.Any(u => u.Id == user.Id))
            return true;

        var listUsers = topic.Users.ToList();
        foreach (var u in listUsers)
        {
            if (u.Id == userId)
            {
                topic.Users.Remove(u);
            }
        }

        _dbContext.Attach(topic).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private IQueryable<Topic> FilterTopics(TopicQuery query)
    {
        IQueryable<Topic> topicQuery = _dbContext.Topics.Include(t => t.SpecificAward)
                                                        .Include(t => t.Leader)
                                                        .Include(t => t.UserTopicRatings)
                                                        .Include(t => t.Users)
                                                        .Include(t => t.TopicRank)
                                                        .Include(t => t.TopicPhotos)
                                                        .Include(t => t.TopicVideos)
                                                        .AsSplitQuery()
                                                        .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.TopicName))
        {
            topicQuery = topicQuery.Where(x => x.TopicName.Contains(query.TopicName));
        }

        if (query.Registered != null)
        {
            topicQuery = topicQuery.Where(x => x.Registered == query.Registered);
        }

        if (query.Cancel != null)
        {
            topicQuery = topicQuery.Where(x => x.Cancel == query.Cancel);
        }

        if (query.ForceLock != null)
        {
            topicQuery = topicQuery.Where(x => x.ForceLock == query.ForceLock);
        }

        if (query?.UserId > 0)
        {
            topicQuery = topicQuery.Where(x => x.Users.Any(u => u.Id == query.UserId));
        }

        if (query?.LeaderId > 0)
        {
            topicQuery = topicQuery.Where(x => x.Leader.Id == query.LeaderId);
        }

        if (query?.RankAwardId > 0)
        {
            topicQuery = topicQuery.Where(x => x.SpecificAward.RankAward.Id == query.RankAwardId);
        }

        if (query?.TopicRankId > 0)
        {
            topicQuery = topicQuery.Where(x => x.TopicRank.Id == query.TopicRankId);
        }

        if (query?.Mark > 0)
        {
            topicQuery = topicQuery.Where(x => x.UserTopicRatings.Sum(r => r.Mark) == query.Mark);
        }

        if (!string.IsNullOrWhiteSpace(query.UrlSlug))
        {
            topicQuery = topicQuery.Where(x => x.UrlSlug == query.UrlSlug);
        }

        if (!string.IsNullOrWhiteSpace(query.RankAwardSlug))
        {
            topicQuery = topicQuery.Where(x => x.SpecificAward.RankAward.UrlSlug == query.RankAwardSlug);
        }

        if (!string.IsNullOrWhiteSpace(query.UserSlug))
        {
            topicQuery = topicQuery.Where(x => x.Users.Any(p => p.UrlSlug == query.UserSlug));
        }

        if (query?.Year > 0)
        {
            topicQuery = topicQuery.Where(x => x.RegisDate.Year == query.Year);
        }

        if (query?.Month > 0)
        {
            topicQuery = topicQuery.Where(x => x.RegisDate.Month == query.Month);
        }

        if (query?.CancelYear > 0)
        {
            topicQuery = topicQuery.Where(x => x.CancelDate.Value.Year == query.CancelYear);
        }

        if (query?.CancelMonth > 0)
        {
            topicQuery = topicQuery.Where(x => x.CancelDate.Value.Month == query.CancelMonth);
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            topicQuery = topicQuery.Where(x => x.TopicName.Contains(query.Keyword) ||
                                             x.ShortDescription.Contains(query.Keyword) ||
                                             x.Description.Contains(query.Keyword) ||
                                             x.SpecificAward.RankAward.AwardName.Contains(query.Keyword) ||
                                             x.Leader.FullName.Contains(query.Keyword) ||
                                             x.TopicRank.RankName.Contains(query.Keyword));
        }

        return topicQuery;
    }

    public async Task<bool> SetOutlineUrlAsync(int topicId, string outlineUrl, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Topics.Where(x => x.Id == topicId)
                                      .ExecuteUpdateAsync(x =>
                                        x.SetProperty(a => a.OutlineUrl, a => outlineUrl),
                                        cancellationToken) > 0;
    }

    public async Task<bool> AddImageUrlAsync(int topicId, string imageUrl, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(new TopicPhoto
        {
            ImageUrl = imageUrl,
            TopicId = topicId
        }, cancellationToken);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddVideoUrlAsync(int topicId, string videoUrl, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(new TopicVideo
        {
            VideoUrl = videoUrl,
            TopicId = topicId
        }, cancellationToken);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveImageUrlAsync(int topicId, string imageUrl, CancellationToken cancellationToken = default)
    {
        var topicPhoto = await _dbContext.TopicPhotos.Where(t => t.TopicId == topicId && t.ImageUrl == imageUrl).FirstOrDefaultAsync(cancellationToken);

        if (topicPhoto == null) return false;

        _dbContext.TopicPhotos.Remove(topicPhoto);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveVideoUrlAsync(int topicId, string videoUrl, CancellationToken cancellationToken = default)
    {
        var topicVideo = await _dbContext.TopicVideos.Where(t => t.TopicId == topicId && t.VideoUrl == videoUrl).FirstOrDefaultAsync(cancellationToken);

        if (topicVideo == null) return false;

        _dbContext.TopicVideos.Remove(topicVideo);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
