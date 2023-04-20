using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace Stpm.WebApi.Models.TopicRank;

public class TopicRankFilterModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Tên hạng mục")]
    public string RankName { get; set; }
    [DisplayName("Slug")]
    public string UrlSlug { get; set; }
    [DisplayName("Topic-Slug")]
    public string TopicSlug { get; set; }
    [DisplayName("RankAward-Slug")]
    public string RankAwardSlug { get; set; }
    [DisplayName("Giải thưởng")]
    public int? RankAwardId { get; set; }
    [DisplayName("Đề tài")]
    public int? TopicId { get; set; }

    public IEnumerable<SelectListItem> TopicList { get; set; }
    public IEnumerable<SelectListItem> RankAwardList { get; set; }
}
