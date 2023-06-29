using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class TopicVideo : IEntity<int>
{
    public int Id { get; set; }

    public string VideoUrl { get; set; }

    public int TopicId { get; set; }

    public virtual Topic Topic { get; set; }
}
