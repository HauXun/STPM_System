namespace Stpm.WebApi.Models.TopicRank;

public class TopicRankEditModel
{
    public int Id { get; set; }
    public string RankName { get; set; }
    public string UrlSlug { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }

    public static async ValueTask<TopicRankEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new TopicRankEditModel()
        {
            Id = int.Parse(form["Id"]),
            RankName = form["RankName"],
            ShortDescription = form["ShortDescription"],
            Description = form["Description"],
        };
    }
}
