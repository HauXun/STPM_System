namespace Stpm.WebApi.Models.Notification;

public class NotificationEditModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Timeline { get; set; }
    public string LevelId { get; set; }

    public static async ValueTask<NotificationEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new NotificationEditModel()
        {
            Id = int.Parse(form["Id"]),
            Title = form["Title"],
            Content = form["Content"],
            Timeline = Convert.ToDateTime(form["Timeline"]),
            LevelId = form["LevelId"],
        };
    }
}
