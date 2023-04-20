using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Models.Comment;

public partial class CommentDto
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Date { get; set; }

    public string UserId { get; set; } = null!;

    public virtual AppUserDto User { get; set; } = null!;

    public virtual ICollection<TopicDto> Topics { get; set; } = new List<TopicDto>();
}
