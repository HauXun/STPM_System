namespace Stpm.Core.DTO.Topic;

public partial class TopicQuery
{
    public string Keyword { get; set; }
    public string TopicName { get; set; }
    public string UrlSlug { get; set; }
    public bool? Registered { get; set; }
    public bool? Cancel { get; set; }
    public bool? ForceLock { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? CancelYear { get; set; }
    public int? CancelMonth { get; set; }
    public string RankAwardSlug { get; set; }
    public string UserSlug { get; set; }
    public int? UserId { get; set; }
    public int? LeaderId { get; set; }
    public int? RankAwardId { get; set; }
    public int? TopicRankId { get; set; }
    public float? Mark { get; set; }
}
