using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace Stpm.WebApi.Models.Comment;

public class CommentFilterModel : PagingModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Năm")]
    public int? Year { get; set; }
    [DisplayName("Tháng")]
    public int? Month { get; set; }
    [DisplayName("Ngày")]
    public int? Day { get; set; }
    [DisplayName("User-Slug")]
    public string UserSlug { get; set; }
    [DisplayName("Topic-Slug")]
    public string TopicSlug { get; set; }
    [DisplayName("Post-Slug")]
    public string PostSlug { get; set; }
    [DisplayName("Tài khoản")]
    public int? UserId { get; set; }
    [DisplayName("Đề tài")]
    public int? TopicId { get; set; }
    [DisplayName("Bài viết")]
    public int? PostId { get; set; }

    public IEnumerable<SelectListItem> UserList { get; set; }
    public IEnumerable<SelectListItem> TopicList { get; set; }
    public IEnumerable<SelectListItem> PostList { get; set; }
    public IEnumerable<SelectListItem> MonthList { get; set; }

    public CommentFilterModel()
    {
        MonthList = Enumerable.Range(1, 12).Select(m => new SelectListItem() { Value = m.ToString(), Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m) }).ToList();
    }
}
