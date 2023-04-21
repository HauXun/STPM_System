namespace Stpm.WebApi.Models.PostPhoto;

public class PostPhotoEditModel
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public IFormFile ImageFile { get; set; }

    public static async ValueTask<PostPhotoEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new PostPhotoEditModel()
        {
            Id = int.Parse(form["Id"]),
            ImageFile = form.Files["ImageFile"],
        };
    }
}
