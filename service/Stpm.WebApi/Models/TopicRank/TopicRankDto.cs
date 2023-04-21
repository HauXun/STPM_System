using Stpm.WebApi.Models.RankAward;
using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Models.TopicRank;

public partial class TopicRankDto
{
    public int Id { get; set; }

    public string RankName { get; set; } = null!;

    public string UrlSlug { get; set; } = null!;

    public string ShortDescription { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<RankAwardDto> RankAwards { get; set; } = new List<RankAwardDto>();

    public virtual ICollection<TopicDto> Topics { get; set; } = new List<TopicDto>();
}
