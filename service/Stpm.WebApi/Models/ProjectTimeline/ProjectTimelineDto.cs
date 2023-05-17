using Stpm.WebApi.Models.Timeline;

namespace Stpm.WebApi.Models.ProjectTimeline;

public class ProjectTimelineDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public bool ShowOn { get; set; }

    //public virtual ICollection<TimelineDto> Timelines { get; set; } = new List<TimelineDto>();
}
