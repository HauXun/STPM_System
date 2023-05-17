using Stpm.Core.Contracts;
using Stpm.Core.DTO.ProjectTimeline;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface IProjectTimelineRepository
{
    Task<IList<ProjectTimeline>> GetProjectTimelinesAsync(CancellationToken cancellationToken = default);

    Task<ProjectTimeline> GetProjectTimelineByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ProjectTimeline> GetCachedProjectTimelineByIdAsync(int projectTimelineId, CancellationToken cancellationToken = default);

    Task<IPagedList<ProjectTimeline>> GetProjectTimelineByQueryAsync(ProjectTimelineQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<IPagedList<ProjectTimeline>> GetProjectTimelineByQueryAsync(ProjectTimelineQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetProjectTimelineByQueryAsync<T>(ProjectTimelineQuery query, IPagingParams pagingParams, Func<IQueryable<ProjectTimeline>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateProjectTimelineAsync(ProjectTimeline projectTimeline, CancellationToken cancellationToken = default);

    Task<bool> DeleteProjectTimelineByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> SwitchProjectTimelineStatusAsync(int id, CancellationToken cancellationToken = default);
}
