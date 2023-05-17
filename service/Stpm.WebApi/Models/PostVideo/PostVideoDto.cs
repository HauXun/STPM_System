using Stpm.WebApi.Models.Post;

namespace Stpm.WebApi.Models.PostVideo;

public class PostVideoDto
{
    public int Id { get; set; }

    public string VideoUrl { get; set; }

    public int PostId { get; set; }

    //public virtual PostDto Post { get; set; }
}
