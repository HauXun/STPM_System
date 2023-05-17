using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.Post;
using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Models.Comment;

public partial class CommentDto
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTime Date { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int UserId { get; set; }

    //public virtual AppUserDto User { get; set; } = null!;

    //public virtual ICollection<TopicDto> Topics { get; set; } = new List<TopicDto>();

    //public virtual ICollection<PostDto> Posts { get; set; } = new List<PostDto>();
}
