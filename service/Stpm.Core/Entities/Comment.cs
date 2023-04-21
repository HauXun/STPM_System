using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class Comment : IEntity<int>
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTime Date { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int UserId { get; set; }

    public virtual AppUser User { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
