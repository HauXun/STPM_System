namespace Stpm.WebApi.Models.Post;

public class PostEditModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string Meta { get; set; }
    public string UrlSlug { get; set; }
    public bool Published { get; set; }
    public string UserId { get; set; }
    public int TopicId { get; set; }
    public string SelectedTags { get; set; }

    // Tách chuỗi chứa các thẻ thành một mảng các chuỗi
    public List<string> GetSelectedTags()
    {
        return (SelectedTags ?? "").Split(new[] { ",", ";", ".", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    public static async ValueTask<PostEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new PostEditModel()
        {
            Id = int.Parse(form["Id"]),
            Title = form["Title"],
            ShortDescription = form["ShortDescription"],
            Description = form["Description"],
            Meta = form["Meta"],
            Published = form["Published"] != "false",
            UserId = form["CategoryId"],
            TopicId = int.Parse(form["TopicId"]),
            SelectedTags = form["SelectedTags"]
        };
    }
}
