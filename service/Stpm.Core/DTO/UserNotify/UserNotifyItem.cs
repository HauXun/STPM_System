namespace Stpm.Core.DTO.UserNotify;

public partial class UserNotifyItem
{
    public string UserId { get; set; }
    public int NotifyId { get; set; }
    public string UserName { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DueDate { get; set; }
    public string LevelName { get; set; }
    public bool Viewed { get; set; }
}
