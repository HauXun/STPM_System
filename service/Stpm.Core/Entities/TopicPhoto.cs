using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class TopicPhoto : IEntity<int>
{
    public int Id { get; set; }

    public string ImageUrl { get; set; }

    public int TopicId { get; set; }

    public virtual Topic Topic { get; set; }
}
