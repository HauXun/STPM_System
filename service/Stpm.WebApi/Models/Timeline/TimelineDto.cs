using Stpm.WebApi.Models.Notification;
using Stpm.WebApi.Models.ProjectTimeline;

namespace Stpm.WebApi.Models.Timeline;

public class TimelineDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public DateTime DueDate { get; set; }

    public int ProjectId { get; set; }

    public virtual ProjectTimelineDto Project { get; set; }

    public virtual ICollection<NotificationDto> Notifies { get; set; } = new List<NotificationDto>();
}
