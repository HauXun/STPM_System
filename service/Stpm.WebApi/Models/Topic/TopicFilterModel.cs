using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace Stpm.WebApi.Models.Topic;

public class TopicFilterModel : PagingModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Tên đề tài")]
    public string TopicName { get; set; }
    [DisplayName("Slug")]
    public string UrlSlug { get; set; }
    [DisplayName("Đã đăng ký")]
    public bool? Registered { get; set; }
    [DisplayName("Đã huỷ")]
    public bool? Cancel { get; set; }
    [DisplayName("Đã khoá")]
    public bool? ForceLock { get; set; }
    [DisplayName("Năm")]
    public int? Year { get; set; }
    [DisplayName("Tháng")]
    public int? Month { get; set; }
    [DisplayName("Năm Huỷ")]
    public int? CancelYear { get; set; }
    [DisplayName("Tháng Huỷ")]
    public int? CancelMonth { get; set; }
    [DisplayName("RankAward-Slug")]
    public string RankAwardSlug { get; set; }
    [DisplayName("User-Slug")]
    public string UserSlug { get; set; }
    [DisplayName("Thí sinh")]
    public int? UserId { get; set; }
    [DisplayName("Trưởng nhóm")]
    public int? LeaderId { get; set; }
    [DisplayName("Giải thưởng")]
    public int? RankAwardId { get; set; }
    [DisplayName("Hạng mục")]
    public int? TopicRankId { get; set; }
    [DisplayName("Điểm đề tài")]
    public float? Mark { get; set; }

    public IEnumerable<SelectListItem> UserList { get; set; }
    public IEnumerable<SelectListItem> RankAwardList { get; set; }
    public IEnumerable<SelectListItem> MonthList { get; set; }

    public TopicFilterModel()
    {
        MonthList = Enumerable.Range(1, 12).Select(m => new SelectListItem() { Value = m.ToString(), Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m) }).ToList();
    }
}
