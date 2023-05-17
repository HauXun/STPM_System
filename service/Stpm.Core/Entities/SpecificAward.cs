using Stpm.Core.Contracts;

namespace Stpm.Core.Entities;

public partial class SpecificAward : IEntity<int>
{
    public int Id { get; set; }

    public int BonusPrize { get; set; }

    public short Year { get; set; }

    public int RankAwardId { get; set; }

    public bool Passed { get; set; }

    public virtual RankAward RankAward { get; set; }

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
