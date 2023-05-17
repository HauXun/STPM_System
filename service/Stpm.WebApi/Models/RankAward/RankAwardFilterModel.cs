using System.ComponentModel;

namespace Stpm.WebApi.Models.RankAward;

public class RankAwardFilterModel : PagingModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Tên giải thưởng")]
    public string AwardName { get; set; }
    [DisplayName("Slug")]
    public string UrlSlug { get; set; }
    [DisplayName("Topic-Slug")]
    public string TopicSlug { get; set; }
    [DisplayName("Đề tài")]
    public int? TopicId { get; set; }
}
