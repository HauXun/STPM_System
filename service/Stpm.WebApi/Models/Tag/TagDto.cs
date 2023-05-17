using Stpm.WebApi.Models.Post;
using System.Text.Json.Serialization;

namespace Stpm.WebApi.Models.Tag;

public partial class TagDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string UrlSlug { get; set; }

    public string Description { get; set; }

    //[JsonIgnore]
    //public virtual ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
}
