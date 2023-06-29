using System.ComponentModel;

namespace Stpm.WebApi.Models.SpecificAward;

public class SpecificAwardFilterModel : PagingModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Tiền thưởng")]
    public int? BonusPrize { get; set; }
    [DisplayName("Năm")]
    public short? Year { get; set; }
    [DisplayName("Đã kết thúc")]
    public bool? Passed { get; set; }
    [DisplayName("RankAward-Slug")]
    public string RankAwardSlug { get; set; }
    [DisplayName("Topic-Slug")]
    public string TopicSlug { get; set; }
    [DisplayName("Đề tài")]
    public int? TopicId { get; set; }
    [DisplayName("Giải thưởng")]
    public int? RankAwardId { get; set; }
}
