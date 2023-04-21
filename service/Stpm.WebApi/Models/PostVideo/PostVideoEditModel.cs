namespace Stpm.WebApi.Models.PostVideo;

public class PostVideoEditModel
{
    public int Id { get; set; }
    public string VideoUrl { get; set; } = null!;
    public IFormFile VideoFile { get; set; }

    public static async ValueTask<PostVideoEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new PostVideoEditModel()
        {
            Id = int.Parse(form["Id"]),
            VideoFile = form.Files["VideoFile"],
        };
    }
}
