namespace Stpm.Core.Entities;

public partial class UserTopicRating
{
    public int UserId { get; set; }

    public int TopicId { get; set; }

    public float? Mark { get; set; }

    public virtual Topic Topic { get; set; }

    public virtual AppUser User { get; set; }
}
