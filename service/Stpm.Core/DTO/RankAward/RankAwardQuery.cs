namespace Stpm.Core.DTO.RankAward;

public partial class RankAwardQuery
{
    public string Keyword { get; set; }
    public string AwardName { get; set; }
    public string UrlSlug { get; set; }
    public string TopicSlug { get; set; }
    public int? TopicId { get; set; }
    public int? Year { get; set; }
}
