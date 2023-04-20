using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.Notification;

namespace Stpm.WebApi.Models.UserNotify;

public partial class UserNotifyDto
{
    public string UserId { get; set; } = null!;

    public int NotifyId { get; set; }

    public bool Viewed { get; set; }

    public virtual NotificationDto Notify { get; set; } = null!;

    public virtual AppUserDto User { get; set; } = null!;
}
