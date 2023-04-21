using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class Tag : IEntity<int>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string UrlSlug { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
