namespace Stpm.WebApi.Models.TopicPhoto;

public class TopicPhotoEditModel
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public IFormFile ImageFile { get; set; }

    public static async ValueTask<TopicPhotoEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new TopicPhotoEditModel()
        {
            Id = int.Parse(form["Id"]),
            ImageFile = form.Files["ImageFile"],
        };
    }
}
