using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class TopicRank : IEntity<int>
{
    public int Id { get; set; }

    public string RankName { get; set; }

    public string UrlSlug { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public virtual ICollection<RankAward> RankAwards { get; set; } = new List<RankAward>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
