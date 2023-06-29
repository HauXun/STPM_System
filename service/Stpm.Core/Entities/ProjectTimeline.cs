using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class ProjectTimeline : IEntity<int>
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public bool ShowOn { get; set; }

    public virtual ICollection<Timeline> Timelines { get; set; } = new List<Timeline>();
}
