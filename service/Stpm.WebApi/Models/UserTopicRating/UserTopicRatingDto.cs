using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Models.UserTopicRating;

public partial class UserTopicRatingDto
{
    public int UserId { get; set; }

    public int TopicId { get; set; }

    public float? Mark { get; set; }

    //public virtual TopicDto Topic { get; set; }

    //public virtual AppUserDto User { get; set; }
}
