using Stpm.WebApi.Models.Post;

namespace Stpm.WebApi.Models.PostPhoto;

public class PostPhotoDto
{
    public int Id { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int PostId { get; set; }

    public virtual PostDto Post { get; set; } = null!;
}
