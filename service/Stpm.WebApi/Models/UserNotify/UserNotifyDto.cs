using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.Notification;

namespace Stpm.WebApi.Models.UserNotify;

public partial class UserNotifyDto
{
    public int UserId { get; set; }

    public int NotifyId { get; set; }

    public bool Viewed { get; set; }

    //public virtual NotificationDto Notify { get; set; }

    //public virtual AppUserDto User { get; set; }
}
