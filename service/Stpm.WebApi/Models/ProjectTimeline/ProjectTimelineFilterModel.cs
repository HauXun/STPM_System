using System.ComponentModel;

namespace Stpm.WebApi.Models.ProjectTimeline;

public class ProjectTimelineFilterModel : PagingModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Tiêu đề")]
    public string Title { get; set; }
    [DisplayName("Hiển thị")]
    public bool? ShowOn { get; set; }
    [DisplayName("Timeline")]
    public int? TimelineId { get; set; }
}
