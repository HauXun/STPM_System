using Stpm.WebApi.Models.NotifyAttachment;
using Stpm.WebApi.Models.NotiLevel;
using Stpm.WebApi.Models.Timeline;
using Stpm.WebApi.Models.UserNotify;

namespace Stpm.WebApi.Models.Notification;

public partial class NotificationDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime? DueDate { get; set; }

    public string LevelId { get; set; }

    public virtual NotiLevelDto Level { get; set; } = null!;

    //public virtual ICollection<TimelineDto> Timelines { get; set; } = new List<TimelineDto>();

    public virtual ICollection<UserNotifyDto> UserNotifies { get; set; } = new List<UserNotifyDto>();

    public virtual ICollection<NotifyAttachmentDto> NotifyAttachments { get; set; } = new List<NotifyAttachmentDto>();
}
