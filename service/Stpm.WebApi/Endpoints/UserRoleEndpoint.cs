using Carter;
using MapsterMapper;
using Stpm.Core.DTO.AppUser;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.AppUser;
using System.Net;

namespace Stpm.WebApi.Endpoints;

public class UserRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/roles");

        routeGroupBuilder.MapGet("/", GetUserRoles)
                         .WithName("GetUserRoles");

        routeGroupBuilder.MapGet("/claims/{id:int}", GetUserRoleClaims)
                         .WithName("GetUserRoleClaims");

        routeGroupBuilder.MapGet("/{id:int}", GetUserRoleById)
                         .WithName("GetUserRoleById");

        routeGroupBuilder.MapPost("/", AddOrEditRole)
                         .WithName("AddOrEditRole")
                         .Accepts<AppUserRoleEditModel>("multipart/form-data");

        routeGroupBuilder.MapPost("/user/{userId:int}", AddOrUpdateUserRole)
                         .WithName("AddOrUpdateUserRole");
    }

    private static async Task<IResult> GetUserRoles(IUserRepository _userRepository)
    {
        var userRoles = await _userRepository.GetRolesAsync();

        return Results.Ok(ApiResponse.Success(userRoles));
    }

    private static async Task<IResult> GetUserRoleClaims(int id, IUserRepository _userRepository)
    {
        var userRoleClaims = await _userRepository.GetRoleClaimsAsync(id);

        return userRoleClaims != null ? Results.Ok(ApiResponse.Success(userRoleClaims)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find claims of role with id = {id}"));
    }

    private static async Task<IResult> GetUserRoleById(int id, IUserRepository _userRepository)
    {
        var role = await _userRepository.GetCachedRoleByIdAsync(id);

        return Results.Ok(ApiResponse.Success(role));
    }

    private static async Task<IResult> AddOrEditRole(HttpContext context, IUserRepository _roleRepository)
    {
        var model = await AppUserRoleEditModel.BindAsync(context);

        var role = model.Id > 0 ? await _roleRepository.GetCachedRoleByIdAsync(model.Id) : null;

        if (role == null)
        {
            role = new AppUserRole();
        }

        role.Name = model.RoleName;

        var resultAdd = await _roleRepository.AddOrEditRoleAsync(role);
        if (!string.IsNullOrEmpty(resultAdd))
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, resultAdd));
        }

        return Results.Ok(ApiResponse.Success(role, HttpStatusCode.Created));
    }

    private static async Task<IResult> AddOrUpdateUserRole(int userId, string[] roles, IUserRepository _userRepository, IMapper mapper)
    {
        var user = await _userRepository.GetCachedUserByIdAsync(userId);

        if (userId <= 0 || user == null)
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find user with id = {userId}"));
        }

        var resultAddRoles = await _userRepository.AddOrEditUserRoleAsync(user, roles);

        if (!string.IsNullOrEmpty(resultAddRoles))
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, resultAddRoles));
        }

        return Results.Ok(ApiResponse.Success(mapper.Map<AppUserItem>(user), HttpStatusCode.Created));
    }
}
