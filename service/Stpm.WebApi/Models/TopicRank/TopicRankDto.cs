using Stpm.WebApi.Models.RankAward;
using Stpm.WebApi.Models.Topic;

namespace Stpm.WebApi.Models.TopicRank;

public partial class TopicRankDto
{
    public int Id { get; set; }

    public string RankName { get; set; }

    public string UrlSlug { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    //public virtual ICollection<RankAwardDto> RankAwards { get; set; } = new List<RankAwardDto>();

    //public virtual ICollection<TopicDto> Topics { get; set; } = new List<TopicDto>();
}
