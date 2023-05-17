namespace Stpm.Core.DTO.Topic;

public partial class TopicItem
{
    public int Id { get; set; }
    public string TopicName { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string UrlSlug { get; set; }
    public string OutlineUrl { get; set; }
    public DateTime RegisDate { get; set; }
    public bool Registered { get; set; }
    public DateTime? CancelDate { get; set; }
    public bool Cancel { get; set; }
    public bool ForceLock { get; set; }

    public string AwardName { get; set; }
    public string TopicRankName { get; set; }
    public string LeaderName { get; set; }
    public int PostCount { get; set; }
    public int UserCount { get; set; }
    public int CommentCount { get; set; }
    public int TopicPhotoCount { get; set; }
    public int TopicVideoCount { get; set; }
    public int TopicRatingCount { get; set; }
    public float? Mark { get; set; }
}
