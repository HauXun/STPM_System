namespace Stpm.Core.DTO.Timeline;

public class TimelineQuery
{
    public string Keyword { get; set; }
    public string Title { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? NotifyId { get; set; }
    public int? ProjectTimelineId { get; set; }
}
