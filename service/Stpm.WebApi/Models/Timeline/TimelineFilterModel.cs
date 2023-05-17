using System.ComponentModel;

namespace Stpm.WebApi.Models.Timeline;

public class TimelineFilterModel : PagingModel
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
    [DisplayName("Project Timeline")]
    public int? ProjectId { get; set; }
}
