using System.ComponentModel;

namespace Stpm.WebApi.Models.ProjectTimeline;

public class ProjectTimelineFilterModel
{
    [DisplayName("Từ khoá")]
    public string Keyword { get; set; }
    [DisplayName("Tiêu đề")]
    public string Title { get; set; }
}
