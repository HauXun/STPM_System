namespace Stpm.Core.DTO.UserTopicRating;

public partial class UserTopicRatingItem
{
    public int UserId { get; set; }
    public int TopicId { get; set; }
    public string UserName { get; set; }
    public int TopicName { get; set; }
    public float? Mark { get; set; }
}
