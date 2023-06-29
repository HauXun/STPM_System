using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.Timeline;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using Stpm.Services.Extensions;

namespace Stpm.Services.App;

public class TimelineRepository : ITimelineRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public TimelineRepository(StpmDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<IList<Timeline>> GetTimelinesAsync(CancellationToken cancellationToken = default)
    {
        var timelineQuery = _dbContext.Timelines.Include(t => t.Notifies)
                                                .Include(t => t.Project)
                                                .AsSplitQuery()
                                                .AsNoTracking();

        return await timelineQuery.ToListAsync(cancellationToken);
    }

    public async Task<Timeline> GetTimelineByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Timelines.Include(t => t.Notifies)
                                         .Include(t => t.Project)
                                         .AsSplitQuery()
                                         .Where(t => t.Id == id)
                                         .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Timeline> GetCachedTimelineByIdAsync(int timelineId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"timeline.by-id.{timelineId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetTimelineByIdAsync(timelineId, cancellationToken);
            });
    }

    public async Task<IPagedList<Timeline>> GetTimelineByQueryAsync(TimelineQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterTimelines(query).ToPagedListAsync(
                                pageNumber,
                                pageSize,
                                nameof(TimelineQuery.Title),
                                "DESC",
                                cancellationToken);
    }

    public async Task<IPagedList<Timeline>> GetTimelineByQueryAsync(TimelineQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await FilterTimelines(query).ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetTimelineByQueryAsync<T>(TimelineQuery query, IPagingParams pagingParams, Func<IQueryable<Timeline>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        IQueryable<T> result = mapper(FilterTimelines(query));

        return await result.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> AddOrUpdateTimelineAsync(Timeline timeline, CancellationToken cancellationToken = default)
    {
        if (timeline.Id > 0)
        {
            _dbContext.Update(timeline);
        }
        else
        {
            await _dbContext.AddAsync(timeline, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteTimelineByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var timeline = await _dbContext.Timelines.FindAsync(id);

        if (timeline is null) return false;

        _dbContext.Timelines.Remove(timeline);
        var rowsCount = await _dbContext.SaveChangesAsync(cancellationToken);

        return rowsCount > 0;
    }

    private IQueryable<Timeline> FilterTimelines(TimelineQuery query)
    {
        IQueryable<Timeline> timelineQuery = _dbContext.Timelines.Include(t => t.Notifies)
                                                                 .Include(t => t.Project)
                                                                 .AsSplitQuery()
                                                                 .AsNoTracking();

        if (query?.NotifyId > 0)
        {
            timelineQuery = timelineQuery.Where(x => x.Notifies.Any(n => n.Id == query.NotifyId));
        }

        if (query?.ProjectTimelineId > 0)
        {
            timelineQuery = timelineQuery.Where(x => x.Project.Id == query.ProjectTimelineId);
        }

        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            timelineQuery = timelineQuery.Where(x => x.Title == query.Title);
        }

        if (query?.Year > 0)
        {
            timelineQuery = timelineQuery.Where(x => x.DueDate.Year == query.Year);
        }

        if (query?.Month > 0)
        {
            timelineQuery = timelineQuery.Where(x => x.DueDate.Month == query.Month);
        }

        if (query?.Day > 0)
        {
            timelineQuery = timelineQuery.Where(x => x.DueDate.Day == query.Day);
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            timelineQuery = timelineQuery.Where(x => x.Title.Contains(query.Keyword) ||
                                             x.Project.Title.Contains(query.Keyword));
        }

        return timelineQuery;
    }
}
