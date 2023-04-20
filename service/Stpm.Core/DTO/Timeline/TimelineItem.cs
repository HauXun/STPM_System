namespace Stpm.Core.DTO.Timeline;

public class TimelineItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public DateTime DueDate { get; set; }
    public string ProjectTimelineName { get; set; }
    public int NotifyCount { get; set; }
}
