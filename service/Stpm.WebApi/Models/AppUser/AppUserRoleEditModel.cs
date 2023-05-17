namespace Stpm.WebApi.Models.AppUser;

public class AppUserRoleEditModel
{
    public int Id { get; set; }
    public string RoleName { get; set; }

    public static async ValueTask<AppUserRoleEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new AppUserRoleEditModel()
        {
            Id = int.Parse(form["Id"]),
            RoleName = form["RoleName"],
        };
    }
}
