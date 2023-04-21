using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace Stpm.WebApi.Models.Notification;

public class NotificationFilterModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Tiêu đề")]
    public string Title { get; set; }
    [DisplayName("Năm")]
    public int? Year { get; set; }
    [DisplayName("Tháng")]
    public int? Month { get; set; }
    [DisplayName("Ngày")]
    public int? Day { get; set; }
    [DisplayName("Độ ưu tiên")]
    public int? LevelId { get; set; }
    [DisplayName("Đã xem")]
    public bool? Viewed { get; set; }

    public IEnumerable<SelectListItem> LevelList { get; set; }
    public IEnumerable<SelectListItem> MonthList { get; set; }

    public NotificationFilterModel()
    {
        MonthList = Enumerable.Range(1, 12).Select(m => new SelectListItem() { Value = m.ToString(), Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m) }).ToList();
    }
}
