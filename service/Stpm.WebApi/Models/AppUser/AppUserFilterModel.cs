using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace Stpm.WebApi.Models.AppUser;

public class AppUserFilterModel : PagingModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Khoá")]
    public bool? LockoutEnable { get; set; }
    [DisplayName("Năm")]
    public int? Year { get; set; }
    [DisplayName("Tháng")]
    public int? Month { get; set; }
    [DisplayName("Tên người dùng")]
    public string UserName { get; set; }
    [DisplayName("Email")]
    public string Email { get; set; }
    [DisplayName("Số điện thoại")]
    public string PhoneNumber { get; set; }
    [DisplayName("Post-Slug")]
    public string PostSlug { get; set; }
    [DisplayName("Topic-Slug")]
    public string TopicSlug { get; set; }
    [DisplayName("User-Slug")]
    public string UserSlug { get; set; }
    [DisplayName("Đề tài")]
    public int? TopicId { get; set; }

    public IEnumerable<SelectListItem> TopicList { get; set; }
    public IEnumerable<SelectListItem> MonthList { get; set; }

    public AppUserFilterModel()
    {
        MonthList = Enumerable.Range(1, 12).Select(m => new SelectListItem() { Value = m.ToString(), Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m) }).ToList();
    }
}
