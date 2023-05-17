using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.PostPhoto;
using Stpm.WebApi.Models.PostVideo;
using Stpm.WebApi.Models.Tag;

namespace Stpm.WebApi.Models.Post;

public partial class PostDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public string Meta { get; set; }

    public string UrlSlug { get; set; }

    public int? ViewCount { get; set; }

    public bool Published { get; set; }

    public DateTime PostedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int UserId { get; set; }

    public virtual AppUserDto User { get; set; }

    public virtual ICollection<TagDto> Tags { get; set; } = new List<TagDto>();

    public virtual ICollection<PostPhotoDto> PostPhotos { get; set; } = new List<PostPhotoDto>();

    public virtual ICollection<PostVideoDto> PostVideos { get; set; } = new List<PostVideoDto>();
}
