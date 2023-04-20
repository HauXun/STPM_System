namespace Stpm.WebApi.Models.TopicHistoryAward;

public partial class TopicHistoryAwardDto
{
    public int Id { get; set; }

    public string TopicName { get; set; } = null!;

    public string UrlSlug { get; set; } = null!;

    public string TopicAward { get; set; } = null!;

    public string TopicRank { get; set; } = null!;

    public short Year { get; set; }

    public float Fullscore { get; set; }
}
