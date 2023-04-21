using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class Timeline : IEntity<int>
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public DateTime DueDate { get; set; }

    public int ProjectId { get; set; }

    public virtual ProjectTimeline Project { get; set; }

    public virtual ICollection<Notification> Notifies { get; set; } = new List<Notification>();
}
