using Newtonsoft.Json;

namespace Stpm.WebApi.Models.AppUser;

public class AppUserEditModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool LockoutEnabled { get; set; }
    public string FullName { get; set; }
    public string ImageUrl { get; set; }
    public IFormFile ImageFile { get; set; }
    public string UrlSlug { get; set; }
    public string MSSV { get; set; }
    public string GradeName { get; set; }
    public string[] Roles { get; set; }

    public static async ValueTask<AppUserEditModel> BindAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        return new AppUserEditModel()
        {
            Id = int.Parse(form["Id"]),
            FullName = form["FullName"],
            ImageFile = form.Files["ImageFile"],
            UserName = form["UserName"],
            Email = form["Email"],
            PhoneNumber = form["PhoneNumber"],
            LockoutEnabled = form["LockoutEnabled"] != "false",
            MSSV = form["MSSV"],
            GradeName = form["GradeName"],
            Roles = form["Roles"].ToString().Split(',').ToArray()
        };
    }
}
