using Stpm.WebApi.Models.Timeline;

namespace Stpm.WebApi.Models.ProjectTimeline;

public class ProjectTimelineDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public virtual ICollection<TimelineDto> Timelines { get; set; } = new List<TimelineDto>();
}
