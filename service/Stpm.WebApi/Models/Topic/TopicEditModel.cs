namespace Stpm.WebApi.Models.Topic;

public class TopicEditModel
{
    public int Id { get; set; }
    public string TopicName { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string UrlSlug { get; set; }
    public IFormFile OutlineFile { get; set; }
    public IFormFileCollection ImageFiles { get; set; }
    public IFormFileCollection VideoFiles { get; set; }
    public string OutlineUrl { get; set; }
    public bool Registered { get; set; }
    public bool Cancel { get; set; }
    public bool ForceLock { get; set; }
    public string RegisTemp { get; set; }
    public int TopicRankId { get; set; }
    public int LeaderId { get; set; }
    public int[] Users { get; set; }
    public int[] UserRatings { get; set; }
    public int? SpecificAwardId { get; set; }

    public static async ValueTask<TopicEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();

        var imageFiles = new FormFileCollection();
        imageFiles.AddRange(form.Files.Where(f => f.Name == "imageFiles"));
        var videoFiles = new FormFileCollection();
        videoFiles.AddRange(form.Files.Where(f => f.Name == "videoFiles"));

        return new TopicEditModel()
        {
            OutlineFile = form.Files["OutlineFile"],
            Id = int.Parse(form["Id"]),
            TopicName = form["TopicName"],
            ShortDescription = form["ShortDescription"],
            Description = form["Description"],
            RegisTemp = form["RegisTemp"],
            Registered = form["Registered"] != "false",
            Cancel = form["Cancel"] != "false",
            ImageFiles = imageFiles,
            VideoFiles = videoFiles,
            ForceLock = form["ForceLock"] != "false",
            TopicRankId = int.Parse(form["TopicRankId"]),
            LeaderId = int.Parse(form["LeaderId"]),
            SpecificAwardId = string.IsNullOrEmpty(form["SpecificAwardId"]) ? null : int.Parse(form["SpecificAwardId"]),
            Users = form["Users"].ToString().Split(',').Select(int.Parse).ToArray(),
            UserRatings = form["UserRatings"].ToString().Split(',').Select(int.Parse).ToArray(),
        };
    }
}
