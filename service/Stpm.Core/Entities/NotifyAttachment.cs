using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class NotifyAttachment : IEntity<int>
{
    public int Id { get; set; }

    public string AttachmentUrl { get; set; }

    public int NotifyId { get; set; }

    public virtual Notification Notify { get; set; }
}
