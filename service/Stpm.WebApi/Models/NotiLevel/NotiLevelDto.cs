using Stpm.WebApi.Models.Notification;

namespace Stpm.WebApi.Models.NotiLevel;

public enum Level
{
    Low,
    Low_Urgency,
    Minor,
    Normal,
    Major,
    Important,
    Public,
    Routine,
    High_Urgency,
    Confidential,
    Emergency,
    Immediate,
    Medium,
    Top,
    High,
    Critical,
    Urgent,
    Express,
    Critical_Urgency,
    Catastrophic,
}

public partial class NotiLevelDto
{
    public string Id { get; set; }

    public string LevelName { get; set; }

    public byte Priority { get; set; }

    public string Description { get; set; }

    //public virtual ICollection<NotificationDto> Notifications { get; set; } = new List<NotificationDto>();
}
