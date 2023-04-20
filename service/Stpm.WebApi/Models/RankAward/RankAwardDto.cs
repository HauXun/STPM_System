using Stpm.WebApi.Models.SpecificAward;
using Stpm.WebApi.Models.TopicRank;

namespace Stpm.WebApi.Models.RankAward;

public partial class RankAwardDto
{
    public int Id { get; set; }

    public string AwardName { get; set; } = null!;

    public string UrlSlug { get; set; } = null!;

    public string ShortDescription { get; set; }

    public string Description { get; set; } = null!;

    public int TopicRankId { get; set; }

    public virtual ICollection<SpecificAwardDto> SpecificAwards { get; set; } = new List<SpecificAwardDto>();

    public virtual TopicRankDto TopicRank { get; set; } = null!;
}
