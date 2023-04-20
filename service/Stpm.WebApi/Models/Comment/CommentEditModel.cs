namespace Stpm.WebApi.Models.Comment;

public class CommentEditModel
{
    public int Id { get; set; }
    public string Content { get; set; }

    public static async ValueTask<CommentEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new CommentEditModel()
        {
            Id = int.Parse(form["Id"]),
            Content = form["Content"],
        };
    }
}
