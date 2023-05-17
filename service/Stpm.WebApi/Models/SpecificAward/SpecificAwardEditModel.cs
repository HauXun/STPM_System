namespace Stpm.WebApi.Models.SpecificAward;

public class SpecificAwardEditModel
{
    public int Id { get; set; }
    public int BonusPrize { get; set; }
    public short Year { get; set; }
    public int RankAwardId { get; set; }
    public bool Passed { get; set; }

    public static async ValueTask<SpecificAwardEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new SpecificAwardEditModel()
        {
            Id = int.Parse(form["Id"]),
            BonusPrize = int.Parse(form["BonusPrize"]),
            Year = short.Parse(form["Year"]),
            Passed = form["Passed"] != "false",
            RankAwardId = int.Parse(form["RankAwardId"]),
        };
    }
}
