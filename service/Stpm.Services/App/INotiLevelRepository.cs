using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface INotiLevelRepository
{
    Task<IList<NotiLevel>> GetNotiLevelsAsync(CancellationToken cancellationToken = default);

    Task<NotiLevel> GetNotiLevelByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<NotiLevel> GetCachedNotiLevelByIdAsync(string notiLevelId, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateNotiLevelAsync(NotiLevel notiLevel, CancellationToken cancellationToken = default);

    Task<bool> DeleteNotiLevelByIdAsync(string id, CancellationToken cancellationToken = default);
}
