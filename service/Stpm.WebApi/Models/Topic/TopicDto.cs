using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.Comment;
using Stpm.WebApi.Models.SpecificAward;
using Stpm.WebApi.Models.TopicPhoto;
using Stpm.WebApi.Models.TopicRank;
using Stpm.WebApi.Models.TopicVideo;
using Stpm.WebApi.Models.UserTopicRating;

namespace Stpm.WebApi.Models.Topic;

public partial class TopicDto
{
    public int Id { get; set; }

    public string TopicName { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public string UrlSlug { get; set; }

    public string OutlineUrl { get; set; }

    public DateTime RegisDate { get; set; }

    public bool Registered { get; set; }

    public DateTime? CancelDate { get; set; }

    public bool Cancel { get; set; }

    public bool ForceLock { get; set; }

    public string RegisTemp { get; set; }

    public int TopicRankId { get; set; }

    public int? SpecificAwardId { get; set; }

    public int LeaderId { get; set; }

#nullable enable
    public virtual SpecificAwardDto? SpecificAward { get; set; }
#nullable disable

    public virtual TopicRankDto TopicRank { get; set; }

    public virtual AppUserDto Leader { get; set; }

    public virtual ICollection<UserTopicRatingDto> UserTopicRatings { get; set; } = new List<UserTopicRatingDto>();

    public virtual ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();

    public virtual ICollection<AppUserDto> Users { get; set; } = new List<AppUserDto>();

    public virtual ICollection<TopicPhotoDto> TopicPhotos { get; set; } = new List<TopicPhotoDto>();

    public virtual ICollection<TopicVideoDto> TopicVideos { get; set; } = new List<TopicVideoDto>();
}
