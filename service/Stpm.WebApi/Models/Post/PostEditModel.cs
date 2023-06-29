namespace Stpm.WebApi.Models.Post;

public class PostEditModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string Meta { get; set; }
    public string UrlSlug { get; set; }
    public IFormFileCollection ImageFiles { get; set; }
    public IFormFileCollection VideoFiles { get; set; }
    public bool Published { get; set; }
    public int UserId { get; set; }
    public string SelectedTags { get; set; }

    // Tách chuỗi chứa các thẻ thành một mảng các chuỗi
    public List<string> GetSelectedTags()
    {
        return (SelectedTags ?? "").Split(new[] { ",", ";", ".", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    public static async ValueTask<PostEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();

        var imageFiles = new FormFileCollection();
        imageFiles.AddRange(form.Files.Where(f => f.Name == "imageFiles"));
        var videoFiles = new FormFileCollection();
        videoFiles.AddRange(form.Files.Where(f => f.Name == "videoFiles"));

        return new PostEditModel()
        {
            Id = int.Parse(form["Id"]),
            Title = form["Title"],
            ShortDescription = form["ShortDescription"],
            Description = form["Description"],
            Meta = form["Meta"],
            ImageFiles = imageFiles,
            VideoFiles = videoFiles,
            Published = form["Published"] != "false",
            UserId = int.Parse(form["UserId"]),
            SelectedTags = form["SelectedTags"]
        };
    }
}
