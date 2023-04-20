using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class Topic : IEntity<int>
{
    public int Id { get; set; }

    public string TopicName { get; set; }

    public string UrlSlug { get; set; }

    public string? OutlineUrl { get; set; }

    public DateTime RegisDate { get; set; }

    public bool Cancel { get; set; }

    public bool ForceLock { get; set; }

    public string? RegisTemp { get; set; }

    public int TopicRankId { get; set; }

    public int? SpecificAwardId { get; set; }

    public virtual SpecificAward? SpecificAward { get; set; }

    public virtual TopicRank TopicRank { get; set; }

    public virtual ICollection<UserTopicRating> UserTopicRatings { get; set; } = new List<UserTopicRating>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();

    public virtual ICollection<TopicPhoto> TopicPhotos { get; set; } = new List<TopicPhoto>();

    public virtual ICollection<TopicVideo> TopicVideos { get; set; } = new List<TopicVideo>();
}
