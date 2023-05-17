namespace Stpm.Core.DTO.TopicRank;

public partial class TopicRankItem
{
    public int Id { get; set; }
    public string RankName { get; set; }
    public string UrlSlug { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
    public int RankAwardCount { get; set; }
    public int TopicCount { get; set; }
}
