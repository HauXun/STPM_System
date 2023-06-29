using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class RankAward : IEntity<int>
{
    public int Id { get; set; }

    public string AwardName { get; set; }

    public string UrlSlug { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public int TopicRankId { get; set; }

    public virtual ICollection<SpecificAward> SpecificAwards { get; set; } = new List<SpecificAward>();

    public virtual TopicRank TopicRank { get; set; }
}
