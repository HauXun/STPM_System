namespace Stpm.WebApi.Models.Notification;

public class NotificationEditModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime? DueDate { get; set; }
    public string LevelId { get; set; }
    public IFormFileCollection Files { get; set; }
    public int[] Users { get; set; }

    public static async ValueTask<NotificationEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();

        var files = new FormFileCollection();
        files.AddRange(form.Files.Where(f => f.Name == "files"));

        return new NotificationEditModel()
        {
            Id = int.Parse(form["Id"]),
            Title = form["Title"],
            Content = form["Content"],
            DueDate = Convert.ToDateTime(form["DueDate"]),
            LevelId = form["LevelId"],
            Files = files,
            Users = form["Users"].ToString().Split(',').Select(int.Parse).ToArray(),
        };
    }
}
