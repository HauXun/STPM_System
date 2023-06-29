using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.ProjectTimeline;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using Stpm.Services.Extensions;

namespace Stpm.Services.App;

public class ProjectTimelineRepository : IProjectTimelineRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public ProjectTimelineRepository(StpmDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<IList<ProjectTimeline>> GetProjectTimelinesAsync(CancellationToken cancellationToken = default)
    {
        var projectTimelineQuery = _dbContext.ProjectTimelines.AsNoTracking();

        return await projectTimelineQuery.ToListAsync(cancellationToken);
    }

    public async Task<ProjectTimeline> GetProjectTimelineByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProjectTimelines.AsSplitQuery()
                                                .Where(t => t.Id == id)
                                                .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProjectTimeline> GetCachedProjectTimelineByIdAsync(int projectTimelineId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"projectTimeline.by-id.{projectTimelineId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetProjectTimelineByIdAsync(projectTimelineId, cancellationToken);
            });
    }

    public async Task<IPagedList<ProjectTimeline>> GetProjectTimelineByQueryAsync(ProjectTimelineQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterProjectTimelines(query).ToPagedListAsync(
                                pageNumber,
                                pageSize,
                                nameof(ProjectTimelineQuery.Title),
                                "DESC",
                                cancellationToken);
    }

    public async Task<IPagedList<ProjectTimeline>> GetProjectTimelineByQueryAsync(ProjectTimelineQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await FilterProjectTimelines(query).ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetProjectTimelineByQueryAsync<T>(ProjectTimelineQuery query, IPagingParams pagingParams, Func<IQueryable<ProjectTimeline>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        IQueryable<T> result = mapper(FilterProjectTimelines(query));

        return await result.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> AddOrUpdateProjectTimelineAsync(ProjectTimeline projectTimeline, CancellationToken cancellationToken = default)
    {
        if (projectTimeline.Id > 0)
        {
            _dbContext.Update(projectTimeline);
        }
        else
        {
            await _dbContext.AddAsync(projectTimeline, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> SwitchProjectTimelineStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var projectTimelines = await _dbContext.ProjectTimelines.FindAsync(id);

        if (projectTimelines == null) return false;

        projectTimelines.ShowOn = !projectTimelines.ShowOn;

        _dbContext.Attach(projectTimelines).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteProjectTimelineByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var projectTimeline = await _dbContext.ProjectTimelines.FindAsync(id);

        if (projectTimeline is null) return false;

        _dbContext.ProjectTimelines.Remove(projectTimeline);
        var rowsCount = await _dbContext.SaveChangesAsync(cancellationToken);

        return rowsCount > 0;
    }

    private IQueryable<ProjectTimeline> FilterProjectTimelines(ProjectTimelineQuery query)
    {
        IQueryable<ProjectTimeline> projectTimelineQuery = _dbContext.ProjectTimelines.AsSplitQuery()
                                                                                      .AsNoTracking();

        if (query?.TimelineId > 0)
        {
            projectTimelineQuery = projectTimelineQuery.Where(x => x.Timelines.Any(t => t.Id == query.TimelineId));
        }

        if (query.ShowOn != null)
        {
            projectTimelineQuery = projectTimelineQuery.Where(x => x.ShowOn == query.ShowOn);
        }

        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            projectTimelineQuery = projectTimelineQuery.Where(x => x.Title == query.Title);
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            projectTimelineQuery = projectTimelineQuery.Where(x => x.ShortDescription.Contains(query.Keyword) ||
                                             x.Timelines.Any(t => t.Title.Contains(query.Keyword)));
        }

        return projectTimelineQuery;
    }
}
