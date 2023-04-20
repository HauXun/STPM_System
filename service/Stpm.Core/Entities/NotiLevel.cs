using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

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

public partial class NotiLevel : IEntity<string>
{
    public string Id { get; set; }

    public string LevelName { get; set; }

    public byte Priority { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
