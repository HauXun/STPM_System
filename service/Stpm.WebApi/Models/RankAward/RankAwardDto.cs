using Stpm.WebApi.Models.SpecificAward;
using Stpm.WebApi.Models.TopicRank;

namespace Stpm.WebApi.Models.RankAward;

public partial class RankAwardDto
{
    public int Id { get; set; }

    public string AwardName { get; set; }

    public string UrlSlug { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public int TopicRankId { get; set; }

    public virtual ICollection<SpecificAwardDto> SpecificAwards { get; set; } = new List<SpecificAwardDto>();

    public virtual TopicRankDto TopicRank { get; set; }
}
