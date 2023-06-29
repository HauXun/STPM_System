namespace Stpm.WebApi.Models.TopicHistoryAward;

public partial class TopicHistoryAwardDto : PagingModel
{
    public int Id { get; set; }

    public string TopicName { get; set; }

    public string UrlSlug { get; set; }

    public string TopicAward { get; set; }

    public string TopicRank { get; set; }

    public short Year { get; set; }

    public float Fullscore { get; set; }
}
