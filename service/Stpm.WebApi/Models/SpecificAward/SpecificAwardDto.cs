using Stpm.WebApi.Models.RankAward;
using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Models.SpecificAward;

public partial class SpecificAwardDto
{
    public int Id { get; set; }

    public int BonusPrize { get; set; }

    public short Year { get; set; }

    public int RankAwardId { get; set; }

    public virtual RankAwardDto RankAward { get; set; } = null!;

    public virtual ICollection<TopicDto> Topics { get; set; } = new List<TopicDto>();
}
