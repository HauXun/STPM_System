namespace Stpm.WebApi.Models.ProjectTimeline;

public class ProjectTimelineEditModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public bool ShowOn { get; set; }

    public static async ValueTask<ProjectTimelineEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new ProjectTimelineEditModel()
        {
            Id = int.Parse(form["Id"]),
            Title = form["Title"],
            ShortDescription = form["ShortDescription"],
            ShowOn = form["ShowOn"] != "false",
        };
    }
}
