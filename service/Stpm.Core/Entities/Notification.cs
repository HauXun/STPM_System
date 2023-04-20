using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class Notification : IEntity<int>
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime? DueDate { get; set; }

    public string LevelId { get; set; }

    public virtual NotiLevel Level { get; set; }

    public virtual ICollection<UserNotify> UserNotifies { get; set; } = new List<UserNotify>();

    public virtual ICollection<Timeline> Timelines { get; set; } = new List<Timeline>();

    public virtual ICollection<NotifyAttachment> NotifyAttachments { get; set; } = new List<NotifyAttachment>();
}
