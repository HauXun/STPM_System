using Microsoft.AspNetCore.Identity;
using Stpm.WebApi.Models.Comment;
using Stpm.WebApi.Models.Post;
using Stpm.WebApi.Models.Topic;
using Stpm.WebApi.Models.UserNotify;
using Stpm.WebApi.Models.UserTopicRating;

namespace Stpm.WebApi.Models.AppUser;

public class AppUserDto : IdentityUser
{
    public string ImageUrl { get; set; }
    public string UrlSlug { get; set; } = null!;
    public DateTime JoinedDate { get; set; }
    public string MSSV { get; set; }

    public virtual ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();

    public virtual ICollection<PostDto> Posts { get; set; } = new List<PostDto>();

    public virtual ICollection<UserNotifyDto> UserNotifies { get; set; } = new List<UserNotifyDto>();

    public virtual ICollection<UserTopicRatingDto> UserTopicRatings { get; set; } = new List<UserTopicRatingDto>();

    public virtual ICollection<TopicDto> Topics { get; set; } = new List<TopicDto>();
}
