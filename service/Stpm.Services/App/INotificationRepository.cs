using Stpm.Core.Contracts;
using Stpm.Core.DTO.Notification;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface INotificationRepository
{
    Task<IList<Notification>> GetNotificationsAsync(CancellationToken cancellationToken = default);

    Task<IList<NotifyAttachment>> GetNotifyAttachmentByIdAsync(int notifyId, CancellationToken cancellationToken = default);

    Task<Notification> GetNotificationByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Notification> GetCachedNotificationByIdAsync(int notificationId, CancellationToken cancellationToken = default);

    Task<IPagedList<Notification>> GetNotificationByQueryAsync(NotificationQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<IPagedList<Notification>> GetNotificationByQueryAsync(NotificationQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetNotificationByQueryAsync<T>(NotificationQuery query, IPagingParams pagingParams, Func<IQueryable<Notification>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateNotificationAsync(Notification notification, CancellationToken cancellationToken = default);

    Task<bool> AddNotificationForUserAsync(int userId, int notifyId, CancellationToken cancellationToken = default);

    Task<bool> AddNotificationForTimelineAsync(int timelineId, int notifyId, CancellationToken cancellationToken = default);

    Task<bool> RemoveNotificationForTimelineAsync(int timelineId, int notifyId, CancellationToken cancellationToken = default);

    Task<bool> DeleteNotificationByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ChangeNotificationStatusAsync(int userId, int notifyId, CancellationToken cancellationToken = default);

    Task<bool> AddAttachmentUrlAsync(int notifyId, string attachmentUrl, CancellationToken cancellationToken = default);

    Task<bool> RemoveAttachmentUrlAsync(int notifyId, string attachmentUrl, CancellationToken cancellationToken = default);
}
