using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.Notification;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using Stpm.Services.Extensions;

namespace Stpm.Services.App;

public class NotificationRepository : INotificationRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public NotificationRepository(StpmDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<IList<Notification>> GetNotificationsAsync(CancellationToken cancellationToken = default)
    {
        var notificationQuery = _dbContext.Notifications.Include(n => n.Level)
                                                        .Include(n => n.NotifyAttachments)
                                                        .Include(n => n.UserNotifies)
                                                        .AsSplitQuery()
                                                        .AsNoTracking();

        return await notificationQuery.ToListAsync(cancellationToken);
    }

    public async Task<Notification> GetNotificationByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notifications.Include(n => n.Level)
                                             .Include(n => n.NotifyAttachments)
                                             .Include(n => n.UserNotifies)
                                             .AsSplitQuery()
                                             .Where(t => t.Id == id)
                                             .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Notification> GetCachedNotificationByIdAsync(int notificationId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"notification.by-id.{notificationId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetNotificationByIdAsync(notificationId, cancellationToken);
            });
    }

    public async Task<IPagedList<Notification>> GetNotificationByQueryAsync(NotificationQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterNotifications(query).ToPagedListAsync(
                                pageNumber,
                                pageSize,
                                nameof(NotificationQuery.Title),
                                "DESC",
                                cancellationToken);
    }

    public async Task<IPagedList<Notification>> GetNotificationByQueryAsync(NotificationQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await FilterNotifications(query).ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetNotificationByQueryAsync<T>(NotificationQuery query, IPagingParams pagingParams, Func<IQueryable<Notification>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        IQueryable<T> result = mapper(FilterNotifications(query));

        return await result.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> AddOrUpdateNotificationAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        if (notification.Id > 0)
        {
            _dbContext.Update(notification);
        }
        else
        {
            await _dbContext.AddAsync(notification, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddNotificationForUserAsync(int userId, int notifyId, CancellationToken cancellationToken = default)
    {
        var existUser = await _dbContext.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        var existNotify = await _dbContext.Notifications.AnyAsync(u => u.Id == notifyId, cancellationToken);

        if (!existUser || !existNotify)
            return false;

        if (await _dbContext.UserNotifies.AnyAsync(u => u.UserId == userId && u.NotifyId == notifyId, cancellationToken))
            return true;

        await _dbContext.UserNotifies.AddAsync(new UserNotify
        {
            UserId = userId,
            NotifyId = notifyId
        }, cancellationToken);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddNotificationForTimelineAsync(int timelineId, int notifyId, CancellationToken cancellationToken = default)
    {
        var timeline = await _dbContext.Timelines.FindAsync(timelineId);
        var notify = await _dbContext.Notifications.FindAsync(notifyId);
        await _dbContext.Entry(timeline).Collection(x => x.Notifies).LoadAsync(cancellationToken);

        if (timeline == null || notify == null) return false;

        if (timeline.Notifies.Contains(notify)) return true;

        timeline.Notifies.Add(notify);

        _dbContext.Attach(timeline).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveNotificationForTimelineAsync(int timelineId, int notifyId, CancellationToken cancellationToken = default)
    {
        var timeline = await _dbContext.Timelines.FindAsync(timelineId);
        var notify = await _dbContext.Notifications.FindAsync(notifyId);
        await _dbContext.Entry(timeline).Collection(x => x.Notifies).LoadAsync(cancellationToken);

        if (timeline == null || notify == null) return false;

        if (!timeline.Notifies.Contains(notify)) return true;

        timeline.Notifies.Remove(notify);
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteNotificationByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var notification = await _dbContext.Notifications.FindAsync(id);

        if (notification is null) return false;

        _dbContext.Notifications.Remove(notification);
        var rowsCount = await _dbContext.SaveChangesAsync(cancellationToken);

        return rowsCount > 0;
    }

    public async Task<IList<NotifyAttachment>> GetNotifyAttachmentByIdAsync(int notifyId, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.NotifyAttachments.AsNoTracking()
                                                .Where(n => n.NotifyId == notifyId);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> ChangeNotificationStatusAsync(int userId, int notifyId, CancellationToken cancellationToken = default)
    {
        var notification = await _dbContext.UserNotifies.Where(n => n.UserId == userId && n.NotifyId == notifyId)
                                                        .FirstOrDefaultAsync(cancellationToken);
        if (notification == null) return false;

        notification.Viewed = !notification.Viewed;

        _dbContext.Attach(notification).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddAttachmentUrlAsync(int notifyId, string attachmentUrl, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(new NotifyAttachment
        {
            NotifyId = notifyId,
            AttachmentUrl = attachmentUrl,
        }, cancellationToken);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveAttachmentUrlAsync(int notifyId, string attachmentUrl, CancellationToken cancellationToken = default)
    {
        var attachment = await _dbContext.NotifyAttachments.Where(t => t.NotifyId == notifyId && t.AttachmentUrl == attachmentUrl).FirstOrDefaultAsync(cancellationToken);

        if (attachment == null) return false;

        _dbContext.NotifyAttachments.Remove(attachment);

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private IQueryable<Notification> FilterNotifications(NotificationQuery query)
    {
        IQueryable<Notification> notificationQuery = _dbContext.Notifications.Include(n => n.Level)
                                                                             .Include(n => n.NotifyAttachments)
                                                                             .Include(n => n.UserNotifies)
                                                                             .AsSplitQuery()
                                                                             .AsNoTracking();

        if (query.Viewed != null)
        {
            notificationQuery = notificationQuery.Where(x => x.UserNotifies.Any(n => n.Viewed == query.Viewed));
        }

        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            notificationQuery = notificationQuery.Where(x => x.Title == query.Title);
        }

        if (!string.IsNullOrWhiteSpace(query.LevelId))
        {
            notificationQuery = notificationQuery.Where(x => x.LevelId == query.LevelId);
        }

        if (query?.Year > 0)
        {
            notificationQuery = notificationQuery.Where(x => x.DueDate.Value.Year == query.Year);
        }

        if (query?.Month > 0)
        {
            notificationQuery = notificationQuery.Where(x => x.DueDate.Value.Month == query.Month);
        }

        if (query?.Day > 0)
        {
            notificationQuery = notificationQuery.Where(x => x.DueDate.Value.Day == query.Day);
        }

        if (query?.UserId > 0)
        {
            notificationQuery = notificationQuery.Where(x => x.UserNotifies.Any(u => u.UserId == query.UserId));
        }

        if (query?.TimelineId > 0)
        {
            notificationQuery = notificationQuery.Where(x => x.Timelines.Any(t => t.Id == query.TimelineId));
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            notificationQuery = notificationQuery.Where(x => x.Content.Contains(query.Keyword) ||
                                                             x.Level.LevelName.Contains(query.Keyword) ||
                                                             x.Timelines.Any(t => t.Title.Contains(query.Keyword)));
        }

        return notificationQuery;
    }
}
