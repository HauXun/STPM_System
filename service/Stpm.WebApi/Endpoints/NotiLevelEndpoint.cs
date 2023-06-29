using Carter;
using MapsterMapper;
using Stpm.Core.DTO.NotiLevel;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.NotiLevel;
using System.Net;

namespace Stpm.WebApi.Endpoints;

public class NotiLevelEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/notilevels");

        routeGroupBuilder.MapGet("/", GetNotiLevels)
                         .WithName("GetNotiLevels");

        routeGroupBuilder.MapGet("/{id}", GetNotiLevelById)
                         .WithName("GetNotiLevelById");

        routeGroupBuilder.MapPost("/", AddOrUpdateNotiLevel)
                         .WithName("AddOrUpdateNotiLevel")
                         .Accepts<NotiLevelEditModel>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id}", DeleteNotiLevel)
                         .WithName("DeleteNotiLevel");
    }

    private static async Task<IResult> GetNotiLevels(INotiLevelRepository notiLevelRepository, IMapper mapper)
    {
        var notiLevelList = await notiLevelRepository.GetNotiLevelsAsync();

        return Results.Ok(ApiResponse.Success(mapper.Map<List<NotiLevelItem>>(notiLevelList)));
    }

    private static async Task<IResult> GetNotiLevelById(string id, INotiLevelRepository notiLevelRepository, IMapper mapper)
    {
        var notiLevel = await notiLevelRepository.GetCachedNotiLevelByIdAsync(id);

        return Results.Ok(ApiResponse.Success(mapper.Map<NotiLevelItem>(notiLevel)));
    }

    private static async Task<IResult> AddOrUpdateNotiLevel(HttpContext context, INotiLevelRepository notiLevelRepository, IMapper mapper)
    {
        var model = await NotiLevelEditModel.BindAsync(context);

        var notiLevel = model.Id != null ? await notiLevelRepository.GetCachedNotiLevelByIdAsync(model.Id) : null;
        if (notiLevel == null)
        {
            notiLevel = new NotiLevel();
        }
        notiLevel.LevelName = model.LevelName;
        notiLevel.Priority = model.Priority;
        notiLevel.Description = model.Description;

        await notiLevelRepository.AddOrUpdateNotiLevelAsync(notiLevel);

        return Results.Ok(ApiResponse.Success(mapper.Map<NotiLevelDto>(notiLevel), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeleteNotiLevel(string id, INotiLevelRepository notiLevelRepository)
    {
        return await notiLevelRepository.DeleteNotiLevelByIdAsync(id) ? Results.Ok(ApiResponse.Success("NotiLevel is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find notiLevel with id = {id}"));
    }
}
