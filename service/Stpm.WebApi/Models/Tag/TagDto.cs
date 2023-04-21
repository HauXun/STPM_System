using Stpm.WebApi.Models.Post;

namespace Stpm.WebApi.Models.Tag;

public partial class TagDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string UrlSlug { get; set; } = null!;

    public string Description { get; set; }

    public virtual ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
}
