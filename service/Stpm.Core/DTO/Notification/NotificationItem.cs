namespace Stpm.Core.DTO.Notification;

public partial class NotificationItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime? DueDate { get; set; }
    public string LevelName { get; set; }
    public int UserCount { get; set; }
    public int TimelineCount { get; set; }
    public int AttachmentCount { get; set; }
}
