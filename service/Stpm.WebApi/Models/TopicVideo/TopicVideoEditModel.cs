namespace Stpm.WebApi.Models.TopicVideo;

public class TopicVideoEditModel
{
    public int Id { get; set; }
    public string VideoUrl { get; set; } = null!;
    public IFormFile VideoFile { get; set; }

    public static async ValueTask<TopicVideoEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new TopicVideoEditModel()
        {
            Id = int.Parse(form["Id"]),
            VideoFile = form.Files["VideoFile"],
        };
    }
}
