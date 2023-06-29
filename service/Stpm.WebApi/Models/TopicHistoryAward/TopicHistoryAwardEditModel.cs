namespace Stpm.WebApi.Models.TopicHistoryAward;

public class TopicHistoryAwardEditModel
{
    public int Id { get; set; }
    public string TopicName { get; set; }
    public string UrlSlug { get; set; }
    public string TopicAward { get; set; }
    public string TopicRank { get; set; }
    public short Year { get; set; }
    public float Fullscore { get; set; }

    public static async ValueTask<TopicHistoryAwardEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();

        return new TopicHistoryAwardEditModel()
        {
            Id = int.Parse(form["Id"]),
            TopicName = form["TopicName"],
            UrlSlug = form["UrlSlug"],
            TopicAward = form["TopicRank"],
            TopicRank = form["TopicRank"],
            Year = short.Parse(form["Year"]),
            Fullscore = float.Parse(form["Fullscore"]),
        };
    }
}
