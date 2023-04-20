namespace Stpm.Core.DTO.NotiLevel;

public partial class NotiLevelItem
{
    public string Id { get; set; }
    public string LevelName { get; set; }
    public byte Priority { get; set; }
    public string Description { get; set; }
    public int NotificationCount { get; set; }
}
