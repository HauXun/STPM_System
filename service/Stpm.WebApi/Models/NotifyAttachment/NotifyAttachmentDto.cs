using Stpm.WebApi.Models.Notification;

namespace Stpm.WebApi.Models.NotifyAttachment;

public class NotifyAttachmentDto
{
    public int Id { get; set; }

    public string AttachmentUrl { get; set; } = null!;

    public int NotifyId { get; set; }

    public virtual NotificationDto Notify { get; set; } = null!;
}
