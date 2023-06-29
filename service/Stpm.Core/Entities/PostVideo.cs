using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class PostVideo : IEntity<int>
{
    public int Id { get; set; }

    public string VideoUrl { get; set; }

    public int PostId { get; set; }

    public virtual Post Post { get; set; }
}
