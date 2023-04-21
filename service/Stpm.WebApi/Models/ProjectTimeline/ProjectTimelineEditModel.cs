namespace Stpm.WebApi.Models.ProjectTimeline;

public class ProjectTimelineEditModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public DateTime DueDate { get; set; }

    public static async ValueTask<ProjectTimelineEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new ProjectTimelineEditModel()
        {
            Id = int.Parse(form["Id"]),
            Title = form["Title"],
            ShortDescription = form["ShortDescription"],
            DueDate = Convert.ToDateTime(form["DueDate"]),
        };
    }
}
