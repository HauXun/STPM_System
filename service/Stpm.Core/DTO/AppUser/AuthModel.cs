namespace Stpm.Core.DTO.AppUser;

public class AuthModel
{
    public TokenModel JwtToken { get; set; }
    public string Message { get; set; }
    public string ReturnUrl { get; set; }
    public string RedirectPath { get; set; }
    public RedirectAction Redirect { get; set; }
}

public class AuthModel<T> : AuthModel
{
    public T Data { get; set; }
}

public enum RedirectAction
{
    RedirectToPage,
    LocalRedirect,
    Page,
}