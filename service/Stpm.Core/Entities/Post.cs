using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class Post : IEntity<int>
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public string Meta { get; set; }

    public string UrlSlug { get; set; }

    public int ViewCount { get; set; }

    public bool Published { get; set; }

    public DateTime PostedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int TopicId { get; set; }

    public int UserId { get; set; }

    public virtual Topic Topic { get; set; }

    public virtual AppUser User { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<PostPhoto> PostPhotos { get; set; } = new List<PostPhoto>();

    public virtual ICollection<PostVideo> PostVideos { get; set; } = new List<PostVideo>();
}
