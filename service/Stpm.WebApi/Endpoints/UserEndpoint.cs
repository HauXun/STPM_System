using Carter;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Stpm.Core.Collections;
using Stpm.Core.DTO.AppUser;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.Services.Media;
using Stpm.WebApi.Extensions;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.Comment;
using Stpm.WebApi.Models.UserTopicRating;
using System.Net;
using System.Security.Policy;
using System.Web;

namespace Stpm.WebApi.Endpoints;

public class UserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/users");

        routeGroupBuilder.MapGet("/", GetUsers)
                         .WithName("GetUsers");

        routeGroupBuilder.MapGet("/{userId:int}/topicrating/{topicId:int}", GetUserTopicRating)
                         .WithName("GetUserTopicRating");

        routeGroupBuilder.MapGet("/{id:int}", GetUserById)
                         .WithName("GetUserById");

        routeGroupBuilder.MapGet("/slug/{slug::regex(^[a-z0-9_-]+$)}", GetUserBySlug)
                         .WithName("GetUserBySlug");

        routeGroupBuilder.MapPost("/", AddOrUpdateUser)
                         .WithName("AddOrUpdateUser")
                         .Accepts<AppUserEditModel>("multipart/form-data");

        routeGroupBuilder.MapPut("/lockenable/switch/{id:int}", SwitchLockEnable)
                         .WithName("SwitchLockEnable");

        routeGroupBuilder.MapPost("/{id:int}/picture", SetUserPicture)
                         .WithName("SetUserPicture")
                         .Accepts<IFormFile>("multipart/form-data");

        routeGroupBuilder.MapPost("/{topicId:int}/reigster", AddUsersByRegisTempAsync)
                         .WithName("AddUsersByRegisTempAsync");

        routeGroupBuilder.MapGet("/get-filter", GetFilter)
                         .WithName("GetUserFilter");

        routeGroupBuilder.MapPost("/login", Login)
                         .WithName("Login");

        routeGroupBuilder.MapPost("/login/token", LoginToken)
                         .WithName("LoginToken");

        routeGroupBuilder.MapPost("/account/external-login/", ExternalLogin)
                         .WithName("ExternalLogin");

        routeGroupBuilder.MapGet("/google-callback/", AccessLogin)
                         .WithName("AccessLogin")
                         .AllowAnonymous();

        routeGroupBuilder.MapPost("/account/confirmation/", Confirmation)
                         .WithName("Confirmation");

        routeGroupBuilder.MapGet("/confirm-email", ConfirmEmail)
                         .WithName("ConfirmEmail");

        routeGroupBuilder.MapGet("/confirm-email-change", ConfirmEmailChange)
                         .WithName("ConfirmEmailChange");
    }

    private static async Task<IResult> GetUsers([AsParameters] AppUserFilterModel model, IUserRepository _userRepository, IMapper mapper)
    {
        var userQuery = mapper.Map<AppUserQuery>(model);
        var usersList = await _userRepository.GetUserByQueryAsync(userQuery, model);

        var paginationResult = new PaginationResult<AppUserItem>(usersList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetUserTopicRating(int userId, int topicId, IUserRepository _userRepository, IMapper mapper, UserManager<AppUser> _userManager)
    {
        var userTopicRating = await _userRepository.GetUserTopicRatingAsync(userId, topicId);
        var result = mapper.Map<UserTopicRatingDto>(userTopicRating);

        return Results.Ok(ApiResponse.Success(result));
    }

    private static async Task<IResult> GetUserById(int id, IUserRepository _userRepository, IMapper mapper, UserManager<AppUser> _userManager)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        var result = mapper.Map<AppUserItem>(user);
        result.Roles = (await _userManager.GetRolesAsync(user)).ToArray();

        return Results.Ok(ApiResponse.Success(result));
    }

    private static async Task<IResult> GetUserBySlug(string slug, IUserRepository _userRepository, IMapper mapper)
    {
        var user = await _userRepository.GetCachedUserBySlugAsync(slug);

        return Results.Ok(ApiResponse.Success(user));
    }

    private static async Task<IResult> AddOrUpdateUser(HttpContext context, IUserRepository _userRepository, IMapper mapper, IMediaManager mediaManager)
    {
        var model = await AppUserEditModel.BindAsync(context);
        var slug = model.FullName.GenerateSlug();

        var user = model.Id > 0 ? await _userRepository.GetCachedUserByIdAsync(model.Id) : null;

        if (user == null)
        {
            user = new AppUser() { JoinedDate = DateTime.Now };
        }

        user.UserName = model.UserName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.LockoutEnabled = model.LockoutEnabled;
        user.FullName = model.FullName;
        user.MSSV = model.MSSV;
        user.GradeName = model.GradeName;
        user.UrlSlug = slug;

        if (model.ImageFile?.Length > 0)
        {
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            string uploadedPath = await mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(), model.ImageFile.FileName, model.ImageFile.ContentType);
            if (!string.IsNullOrWhiteSpace(uploadedPath))
            {
                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    string decodeUrl = HttpUtility.UrlDecode(user.ImageUrl);
                    var uri = new Uri(decodeUrl);

                    var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));
                }

                user.ImageUrl = hostname + uploadedPath;
            }
        }

        var resultAdd = await _userRepository.AddOrUpdateUserAsync(user);

        if (!resultAdd)
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, $"Could not add users"));
        }

        var userItem = mapper.Map<AppUserItem>(user);

        if (resultAdd && model?.Roles?.Length > 0)
        {
            var resultAddRoles = await _userRepository.AddOrEditUserRoleAsync(user, model.Roles);
            if (!string.IsNullOrEmpty(resultAddRoles))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, resultAddRoles));
            }

            userItem.Roles = model.Roles;
        }

        return Results.Ok(ApiResponse.Success(userItem, HttpStatusCode.Created));
    }

    private static async Task<IResult> SwitchLockEnable(int id, IUserRepository _userRepository)
    {
        return await _userRepository.SwitchUserStatusAsync(id) ? Results.Ok(ApiResponse.Success("User is switch lockenable", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find user with id = {id}"));
    }

    private static async Task<IResult> SetUserPicture(int id, IFormFile formFile, HttpContext context, IUserRepository _userRepository, IMediaManager mediaManager)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find user with id = {id}"));
        }

        var newPath = string.Empty;
        if (formFile?.Length > 0)
        {
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            string uploadedPath = await mediaManager.SaveFileAsync(formFile.OpenReadStream(), formFile.FileName, formFile.ContentType);
            if (!string.IsNullOrWhiteSpace(uploadedPath))
            {
                // Nếu lưu thành công, xóa tập tin hình ảnh cũ (nếu có)
                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    string decodeUrl = HttpUtility.UrlDecode(user.ImageUrl);
                    var uri = new Uri(decodeUrl);

                    var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));
                }

                newPath = hostname + uploadedPath;
                await _userRepository.SetImageUrlAsync(id, newPath);
            }
        }

        return Results.Ok(ApiResponse.Success(newPath));
    }

    private static async Task<IResult> AddUsersByRegisTempAsync(int topicId, string regisData, IUserRepository _userRepository, IMapper mapper)
    {
        var result = await _userRepository.AddUsersByRegisTempAsync(topicId, regisData);

        return result != null ? Results.Ok(ApiResponse.Success(mapper.Map<List<AppUserItem>>(result))) : Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, $"Could not add users for this topic"));
    }

    private static async Task<IResult> GetFilter(ITopicRepository topicRepository, IPostRepository postRepository)
    {
        var model = new AppUserFilterModel()
        {
            TopicList = (await topicRepository.GetTopicsAsync())
                        .Select(c => new SelectListItem()
                        {
                            Text = c.TopicName,
                            Value = c.Id.ToString()
                        }),

            PostList = (await postRepository.GetPostsAsync())
                        .Select(c => new SelectListItem()
                        {
                            Text = c.Title,
                            Value = c.Id.ToString()
                        })
        };

        return Results.Ok(ApiResponse.Success(model));
    }

    private static async Task<IResult> LoginToken(string token, string refreshToken, IUserRepository _userRepository, IMapper mapper)
    {
        var result = await _userRepository.TokenLoginAsync(token, refreshToken);
        return Results.Ok(ApiResponse.Success(result));
    }

    private static async Task<IResult> Login(string usernameOrEmail, string password, IUserRepository _userRepository, IMapper mapper)
    {
        var result = await _userRepository.LoginAsync(usernameOrEmail, password, false);
        return Results.Ok(ApiResponse.Success(mapper.Map<TokenModel>(result)));
    }

    private static async Task<IActionResult> ExternalLogin(HttpContext context, SignInManager<AppUser> _signInManager)
    {
        var redirectUrl = "https://localhost:7150/api/users/google-callback"; // The URL that Google will redirect back to after the user logs in

        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

        // Start the Google authentication flow
        await context.ChallengeAsync("Google", properties);

        // Return a 401 Unauthorized status code to indicate that the user needs to authenticate with Google
        return new EmptyResult();
    }

    private static async Task<IResult> AccessLogin(string returnUrl, string remoteError, HttpContext context, IUserRepository _userRepository, IMapper mapper)
    {
        returnUrl = returnUrl ?? "/";
        //var userIdentity = context.User.Identity;

        // Complete the external authentication process
        //var result = await context.AuthenticateAsync(IdentityConstants.ExternalScheme);

        //if (!result.Succeeded)
        //{
        //    // Handle the authentication failure
        //}

        // Get the user's email address and check if they are registered with your application
        //var email = result.Principal.FindFirstValue(ClaimTypes.Email);

        //var user = await signInManager.UserManager.FindByEmailAsync(email);

        var result = await _userRepository.ExternalLoginCallbackAsync(returnUrl, remoteError);

        return Results.Ok(ApiResponse.Success(result));
    }

    private async Task<IResult> Confirmation(string email, string returnUrl, IUserRepository _userRepository, IMapper mapper)
    {
        returnUrl = returnUrl ?? "/";

        var result = await _userRepository.ConfirmExternalLoginAsync(returnUrl, email);

        return Results.Ok(ApiResponse.Success(result));
    }

    private async Task<IResult> ConfirmEmail(int userId, string code, IUserRepository _userRepository, IMapper mapper)
    {
        if (userId <= 0 || code == null)
        {
            // redirect to page index
            return Results.Ok(ApiResponse.Success(new AuthModel
            {
                Redirect = RedirectAction.RedirectToPage,
                RedirectPath = "/",
            }));
        }

        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy người dùng có mã số {userId}"));
        }

        var result = await _userRepository.ConfirmEmailAsync(user, code);

        return Results.Ok(ApiResponse.Success(result));
    }

    private async Task<IResult> ConfirmEmailChange(int userId, string email, string code, IUserRepository _userRepository, IMapper mapper)
    {
        if (userId <= 0 || code == null || email == null)
        {
            // redirect to page index
            return Results.Ok(ApiResponse.Success(new AuthModel
            {
                Redirect = RedirectAction.RedirectToPage,
                RedirectPath = "/",
            }));
        }

        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy người dùng có mã số {userId}"));
        }

        var result = await _userRepository.ConfirmEmailChangeAsync(user, code, email);

        return Results.Ok(ApiResponse.Success(result));
    }

}
