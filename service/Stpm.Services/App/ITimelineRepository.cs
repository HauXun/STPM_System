using Stpm.Core.Contracts;
using Stpm.Core.DTO.Timeline;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface ITimelineRepository
{
    Task<IList<Timeline>> GetTimelinesAsync(CancellationToken cancellationToken = default);

    Task<Timeline> GetTimelineByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Timeline> GetCachedTimelineByIdAsync(int timelineId, CancellationToken cancellationToken = default);

    Task<IPagedList<Timeline>> GetTimelineByQueryAsync(TimelineQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<IPagedList<Timeline>> GetTimelineByQueryAsync(TimelineQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetTimelineByQueryAsync<T>(TimelineQuery query, IPagingParams pagingParams, Func<IQueryable<Timeline>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateTimelineAsync(Timeline timeline, CancellationToken cancellationToken = default);

    Task<bool> DeleteTimelineByIdAsync(int id, CancellationToken cancellationToken = default);
}
