namespace Stpm.WebApi.Models.AppUser;

public class AppUserEditModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool LockoutEnabled { get; set; }
    public string ImageUrl { get; set; }
    public IFormFile ImageFile { get; set; }
    public string UrlSlug { get; set; }
    public string MSSV { get; set; }

    public static async ValueTask<AppUserEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new AppUserEditModel()
        {
            Id = form["Id"],
            ImageFile = form.Files["ImageFile"],
            UserName = form["UserName"],
            Email = form["Email"],
            PhoneNumber = form["PhoneNumber"],
            LockoutEnabled = form["LockoutEnabled"] != "false",
            MSSV = form["MSSV"],
        };
    }
}
