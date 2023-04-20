namespace Stpm.Core.Entities;

public partial class UserNotify
{
    public int UserId { get; set; }

    public int NotifyId { get; set; }

    public bool Viewed { get; set; }

    public virtual Notification Notify { get; set; }

    public virtual AppUser User { get; set; }
}
