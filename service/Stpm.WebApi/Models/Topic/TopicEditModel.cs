namespace Stpm.WebApi.Models.Topic;

public class TopicEditModel
{
    public int Id { get; set; }
    public string TopicName { get; set; }
    public string UrlSlug { get; set; }
    public IFormFile OutlineFile { get; set; }
    public string OutlineUrl { get; set; }
    public bool Cancel { get; set; }
    public bool ForceLock { get; set; }
    public string RegisTemp { get; set; }
    public int TopicRankId { get; set; }
    public int? SpecificAwardId { get; set; }

    public static async ValueTask<TopicEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new TopicEditModel()
        {
            OutlineFile = form.Files["OutlineFile"],
            Id = int.Parse(form["Id"]),
            TopicName = form["TopicName"],
            RegisTemp = form["RegisTemp"],
            Cancel = form["Cancel"] != "false",
            ForceLock = form["ForceLock"] != "false",
            TopicRankId = int.Parse(form["TopicRankId"]),
            SpecificAwardId = int.Parse(form["SpecificAwardId"]),
        };
    }
}
