namespace Stpm.WebApi.Models.Timeline;

public class TimelineEditModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public DateTime DueDate { get; set; }
    public int ProjectId { get; set; }
    public int[] Notifies { get; set; }

    public static async ValueTask<TimelineEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new TimelineEditModel()
        {
            Id = int.Parse(form["Id"]),
            Title = form["Title"],
            ShortDescription = form["ShortDescription"],
            DueDate = Convert.ToDateTime(form["DueDate"]),
            ProjectId = int.Parse(form["ProjectId"]),
            Notifies = form["Notifies"].ToString().Split(',').Select(int.Parse).ToArray(),
        };
    }
}
