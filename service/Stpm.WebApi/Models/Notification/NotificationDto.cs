using Stpm.WebApi.Models.NotiLevel;
using Stpm.WebApi.Models.UserNotify;

namespace Stpm.WebApi.Models.Notification;

public partial class NotificationDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime Timeline { get; set; }

    public string LevelId { get; set; } = null!;

    public virtual NotiLevelDto Level { get; set; } = null!;

    public virtual ICollection<UserNotifyDto> UserNotifies { get; set; } = new List<UserNotifyDto>();
}
