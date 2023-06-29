namespace Stpm.Core.DTO.ProjectTimeline;

public class ProjectTimelineQuery
{
    public string Keyword { get; set; }
    public string Title { get; set; }
    public bool? ShowOn { get; set; }
    public int? TimelineId { get; set; }
}
