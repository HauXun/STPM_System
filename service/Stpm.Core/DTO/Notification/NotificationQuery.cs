namespace Stpm.Core.DTO.Notification;

public partial class NotificationQuery
{
    public string Keyword { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public string LevelId { get; set; }
    public string LevelName { get; set; }
    public bool? Viewed { get; set; }
}
