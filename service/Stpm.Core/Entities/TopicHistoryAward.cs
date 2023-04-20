using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class TopicHistoryAward : IEntity<int>
{
    public int Id { get; set; }

    public string TopicName { get; set; }

    public string UrlSlug { get; set; }

    public string TopicAward { get; set; }

    public string TopicRank { get; set; }

    public short Year { get; set; }

    public float Fullscore { get; set; }
}
