using Microsoft.AspNetCore.Identity;
using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public class AppUser : IdentityUser<int>, IEntity<int>
{
    public string FullName { get; set; }
    public string ImageUrl { get; set; }
    public string UrlSlug { get; set; }
    public DateTime JoinedDate { get; set; }
    public string MSSV { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<UserNotify> UserNotifies { get; set; } = new List<UserNotify>();

    public virtual ICollection<UserTopicRating> UserTopicRatings { get; set; } = new List<UserTopicRating>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
