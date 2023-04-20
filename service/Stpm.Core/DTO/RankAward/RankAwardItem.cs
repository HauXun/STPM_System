namespace Stpm.Core.DTO.RankAward;

public partial class RankAwardItem
{
    public int Id { get; set; }
    public string AwardName { get; set; }
    public string UrlSlug { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public int TopicRankName { get; set; }
    public int SpecificAwardCount { get; set; }
}
