using Stpm.Core.Contracts;
using Stpm.Core.DTO.RankAward;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface IRankAwardRepository
{
    Task<IList<RankAward>> GetRankAwardsAsync(CancellationToken cancellationToken = default);

    Task<RankAward> GetRankAwardByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<RankAward> GetCachedRankAwardByIdAsync(int rankAwardId, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateRankAwardAsync(RankAward rankAward, CancellationToken cancellationToken = default);

    Task<SpecificAward> GetSpecificAwardByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<SpecificAward> GetCachedSpecificAwardByIdAsync(int specificAwardId, CancellationToken cancellationToken = default);

    Task<IPagedList<RankAward>> GetRankAwardByQueryAsync(RankAwardQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<IPagedList<RankAward>> GetRankAwardByQueryAsync(RankAwardQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetRankAwardByQueryAsync<T>(RankAwardQuery query, IPagingParams pagingParams, Func<IQueryable<RankAward>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateSpecificAwardAsync(SpecificAward specificAward, CancellationToken cancellationToken = default);

    Task<bool> RemoveSpecificAwardAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> DeleteRankAwardByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> IsExistAwardSpecificationAsync(int bonusPrize, short year, int rankAwardId, CancellationToken cancellationToken = default);

    Task<bool> SwitchPassedStatusAsync(short year, CancellationToken cancellationToken = default);
}
