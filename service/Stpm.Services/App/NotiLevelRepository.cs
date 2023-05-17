using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;

namespace Stpm.Services.App;

public class NotiLevelRepository : INotiLevelRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public NotiLevelRepository(StpmDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<IList<NotiLevel>> GetNotiLevelsAsync(CancellationToken cancellationToken = default)
    {
        var notiLevelQuery = _dbContext.NotiLevels.Include(x => x.Notifications)
                                                  .AsSplitQuery()
                                                  .AsNoTracking();

        return await notiLevelQuery.ToListAsync(cancellationToken);
    }

    public async Task<NotiLevel> GetNotiLevelByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.NotiLevels.Include(x => x.Notifications)
                                          .AsSplitQuery()
                                          .Where(t => t.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<NotiLevel> GetCachedNotiLevelByIdAsync(string notiLevelId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"notiLevel.by-id.{notiLevelId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetNotiLevelByIdAsync(notiLevelId, cancellationToken);
            });
    }

    public async Task<bool> AddOrUpdateNotiLevelAsync(NotiLevel notiLevel, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(notiLevel.Id))
        {
            _dbContext.Update(notiLevel);
        }
        else
        {
            await _dbContext.AddAsync(notiLevel, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteNotiLevelByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var notiLevel = await _dbContext.NotiLevels.FindAsync(id);

        if (notiLevel is null) return false;

        _dbContext.NotiLevels.Remove(notiLevel);
        var rowsCount = await _dbContext.SaveChangesAsync(cancellationToken);

        return rowsCount > 0;
    }
}
