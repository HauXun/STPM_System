namespace Stpm.WebApi.Models.NotifyAttachment;

public class NotifyAttachmentEditModel
{
    public int Id { get; set; }
    public string AttachmentUrl { get; set; } = null!;
    public IFormFile AttachmentFile { get; set; }

    public static async ValueTask<NotifyAttachmentEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new NotifyAttachmentEditModel()
        {
            Id = int.Parse(form["Id"]),
            AttachmentFile = form.Files["AttachmentFile"],
        };
    }
}
