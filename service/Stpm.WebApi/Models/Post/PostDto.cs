using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.PostPhoto;
using Stpm.WebApi.Models.PostVideo;
using Stpm.WebApi.Models.Tag;
using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Models.Post;

public partial class PostDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Meta { get; set; }

    public string UrlSlug { get; set; }

    public int? ViewCount { get; set; }

    public bool Published { get; set; }

    public DateTime PostedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int TopicId { get; set; }

    public string UserId { get; set; } = null!;

    public virtual TopicDto Topic { get; set; } = null!;

    public virtual AppUserDto User { get; set; } = null!;

    public virtual ICollection<TagDto> Tags { get; set; } = new List<TagDto>();

    public virtual ICollection<PostPhotoDto> PostPhotos { get; set; } = new List<PostPhotoDto>();

    public virtual ICollection<PostVideoDto> PostVideos { get; set; } = new List<PostVideoDto>();
}
