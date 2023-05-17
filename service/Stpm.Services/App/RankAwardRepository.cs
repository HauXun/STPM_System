using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.DTO.RankAward;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;

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
}
