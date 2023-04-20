namespace Stpm.WebApi.Models.RankAward;

public class RankAwardEditModel
{
    public int Id { get; set; }
    public string AwardName { get; set; }
    public string UrlSlug { get; set; }
    public string? ShortDescription { get; set; }
    public string Description { get; set; } 
    public int TopicRankId { get; set; }

    public static async ValueTask<RankAwardEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new RankAwardEditModel()
        {
            Id = int.Parse(form["Id"]),
            AwardName = form["Title"],
            ShortDescription = form["ShortDescription"],
            Description = form["Description"],
            TopicRankId = int.Parse(form["TopicRankId"]),
        };
    }
}
