using Stpm.WebApi.Extensions;

namespace Stpm.WebApi.Models.NotiLevel;

public class NotiLevelEditModel
{
    public string Id { get; set; }
    public string LevelName { get; set; }
    public byte Priority { get; set; }
    public string Description { get; set; }

    public static async ValueTask<NotiLevelEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new NotiLevelEditModel()
        {
            Id = form["LevelName"].ToString().GenerateSlug(),
            LevelName = form["LevelName"],
            Priority = Convert.ToByte(form["Priority"]),
            Description = form["Description"],
        };
    }
}
