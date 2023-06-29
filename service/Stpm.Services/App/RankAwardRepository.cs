using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.Post;
using Stpm.Core.DTO.RankAward;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using Stpm.Services.Extensions;

namespace Stpm.Services.App;

public class RankAwardRepository : IRankAwardRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public RankAwardRepository(StpmDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<IList<RankAward>> GetRankAwardsAsync(CancellationToken cancellationToken = default)
    {
        var rankAwardQuery = _dbContext.RankAwards.Include(r => r.TopicRank).Include(t => t.SpecificAwards)
                                                  .AsSplitQuery()
                                                  .AsNoTracking();

        return await rankAwardQuery.ToListAsync(cancellationToken);
    }

    public async Task<RankAward> GetRankAwardByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RankAwards.Include(t => t.SpecificAwards)
                                          .AsSplitQuery()
                                          .Where(t => t.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<RankAward> GetCachedRankAwardByIdAsync(int rankAwardId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"rankAward.by-id.{rankAwardId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetRankAwardByIdAsync(rankAwardId, cancellationToken);
            });
    }

    public async Task<bool> AddOrUpdateRankAwardAsync(RankAward rankAward, CancellationToken cancellationToken = default)
    {
        if (rankAward.Id > 0)
        {
            _dbContext.Update(rankAward);
        }
        else
        {
            await _dbContext.AddAsync(rankAward, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<SpecificAward> GetSpecificAwardByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SpecificAwards.AsSplitQuery()
                                              .AsNoTracking()
                                              .Where(t => t.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SpecificAward> GetCachedSpecificAwardByIdAsync(int specificAwardId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"specificAward.by-id.{specificAwardId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetSpecificAwardByIdAsync(specificAwardId, cancellationToken);
            });
    }

    public async Task<IPagedList<RankAward>> GetRankAwardByQueryAsync(RankAwardQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterRankAward(query).ToPagedListAsync(
                                            pageNumber,
                                            pageSize,
                                            nameof(RankAward.Id),
                                            "DESC",
                                            cancellationToken);
    }

    public async Task<IPagedList<RankAward>> GetRankAwardByQueryAsync(RankAwardQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await FilterRankAward(query).ToPagedListAsync(
                                            pagingParams,
                                            cancellationToken);
    }

    public async Task<IPagedList<T>> GetRankAwardByQueryAsync<T>(RankAwardQuery query, IPagingParams pagingParams, Func<IQueryable<RankAward>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        IQueryable<T> result = mapper(FilterRankAward(query));

        return await result.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> AddOrUpdateSpecificAwardAsync(SpecificAward specificAward, CancellationToken cancellationToken = default)
    {
        if (specificAward.Id > 0)
        {
            _dbContext.Update(specificAward);
        }
        else
        {
            await _dbContext.AddAsync(specificAward, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveSpecificAwardAsync(int id, CancellationToken cancellationToken = default)
    {
        var specificAward = await _dbContext.SpecificAwards.FindAsync(id);

        if (specificAward is null) return false;

        _dbContext.SpecificAwards.Remove(specificAward);
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteRankAwardByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var rankAward = await _dbContext.RankAwards.FindAsync(id);

        if (rankAward is null) return false;

        _dbContext.RankAwards.Remove(rankAward);
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> IsExistAwardSpecificationAsync(int bonusPrize, short year, int rankAwardId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SpecificAwards.AnyAsync(x => x.BonusPrize == bonusPrize && x.Year == year && x.RankAwardId == rankAwardId, cancellationToken);
    }

    public async Task<bool> SwitchPassedStatusAsync(short year, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SpecificAwards.Where(x => x.Year == year)
                                              .ExecuteUpdateAsync(p => p.SetProperty(x => x.Passed, x => !x.Passed), cancellationToken) > 0;
    }

    private IQueryable<RankAward> FilterRankAward(RankAwardQuery query)
    {
        IQueryable<RankAward> rankAwardQuery = _dbContext.RankAwards.Include(r => r.TopicRank)
                                                                    .Include(t => t.SpecificAwards)
                                                                    .AsSplitQuery()
                                                                    .AsNoTracking();

        if (query?.TopicId > 0)
        {
            rankAwardQuery = rankAwardQuery.Where(x => x.TopicRank.Id == query.TopicId);
        }

        if (!string.IsNullOrWhiteSpace(query.TopicSlug))
        {
            rankAwardQuery = rankAwardQuery.Where(x => x.TopicRank.UrlSlug== query.UrlSlug);
        }

        if (!string.IsNullOrWhiteSpace(query.UrlSlug))
        {
            rankAwardQuery = rankAwardQuery.Where(x => x.UrlSlug == query.UrlSlug);
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            rankAwardQuery = rankAwardQuery.Where(x => x.AwardName.Contains(query.Keyword));
        }

        if (query?.Year > 0)
        {
            rankAwardQuery = rankAwardQuery.Where(x => x.SpecificAwards.Any(s => s.Year == query.Year));
        }

        return rankAwardQuery;
    }
}
