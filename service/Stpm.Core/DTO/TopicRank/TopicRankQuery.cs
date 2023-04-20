namespace Stpm.Core.DTO.TopicRank;

public partial class TopicRankQuery
{
    public string Keyword { get; set; }
    public string RankName { get; set; }
    public string UrlSlug { get; set; }
    public string TopicSlug { get; set; }
    public string RankAwardSlug { get; set; }
    public int? RankAwardId { get; set; }
    public int? TopicId { get; set; }
}
