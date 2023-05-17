using Carter;
using MapsterMapper;
using Stpm.Core.DTO.RankAward;
using Stpm.Core.DTO.SpecificAward;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.WebApi.Extensions;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.RankAward;
using Stpm.WebApi.Models.SpecificAward;
using System.Net;

namespace Stpm.WebApi.Endpoints;

public class RankAwardEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/rankaward");

        // Nested Map with defined specific route
        routeGroupBuilder.MapGet("/", GetRankAwards)
                         .WithName("GetRankAwards");

        routeGroupBuilder.MapGet("/{id:int}", GetRankAwardById)
                         .WithName("GetRankAwardById");

        routeGroupBuilder.MapPost("/", AddOrUpdateRankAward)
                         .WithName("AddOrUpdateRankAward")
                         .Accepts<RankAwardEditModel>("multipart/form-data");

        routeGroupBuilder.MapPost("/specific", AddOrUpdateSpecificAward)
                         .WithName("AddOrUpdateSpecificAward")
                         .Accepts<SpecificAwardEditModel>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id:int}", DeleteRankAward)
                         .WithName("DeleteRankAward");

        routeGroupBuilder.MapDelete("/specific/{id:int}", RemoveSpecificAward)
                         .WithName("RemoveSpecificAward");

        routeGroupBuilder.MapPut("/passed/switch/{year}", SwitchPassed)
                         .WithName("SwitchPassed");
    }

    private static async Task<IResult> GetRankAwards(IRankAwardRepository rankAwardRepository, IMapper mapper)
    {
        var rankAwardsList = await rankAwardRepository.GetRankAwardsAsync();

        return Results.Ok(ApiResponse.Success(mapper.Map<List<RankAwardDto>>(rankAwardsList)));
    }

    private static async Task<IResult> GetRankAwardById(int id, IRankAwardRepository rankAwardRepository, IMapper mapper)
    {
        var rankAward = await rankAwardRepository.GetCachedRankAwardByIdAsync(id);

        return rankAward == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}")) : Results.Ok(ApiResponse.Success(mapper.Map<RankAwardDto>(rankAward)));
    }

    private static async Task<IResult> AddOrUpdateRankAward(HttpContext context, IRankAwardRepository rankAwardRepository, IMapper mapper)
    {
        var model = await RankAwardEditModel.BindAsync(context);

        var rankAward = model.Id > 0 ? await rankAwardRepository.GetRankAwardByIdAsync(model.Id) : null;
        if (rankAward == null)
        {
            rankAward = new RankAward();
        }

        rankAward.AwardName = model.AwardName;
        rankAward.ShortDescription = model.ShortDescription;
        rankAward.Description = model.Description;
        rankAward.TopicRankId = model.TopicRankId;
        rankAward.UrlSlug = model.AwardName.GenerateSlug();

        await rankAwardRepository.AddOrUpdateRankAwardAsync(rankAward);

        return Results.Ok(ApiResponse.Success(mapper.Map<RankAwardItem>(rankAward), HttpStatusCode.Created));
    }

    private static async Task<IResult> AddOrUpdateSpecificAward(HttpContext context, IRankAwardRepository rankAwardRepository, IMapper mapper)
    {
        var model = await SpecificAwardEditModel.BindAsync(context);

        if (await rankAwardRepository.IsExistAwardSpecificationAsync(model.BonusPrize, model.Year, model.RankAwardId))
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Giải thưởng này đã được thiết lập"));
        }

        var specificAward = model.Id > 0 ? await rankAwardRepository.GetCachedSpecificAwardByIdAsync(model.Id) : null;
        if (specificAward == null)
        {
            specificAward = new SpecificAward();
        }

        specificAward.BonusPrize = model.BonusPrize;
        specificAward.Year = model.Year;
        specificAward.RankAwardId = model.RankAwardId;
        specificAward.Passed = model.Passed;

        await rankAwardRepository.AddOrUpdateSpecificAwardAsync(specificAward);

        return Results.Ok(ApiResponse.Success(mapper.Map<SpecificAwardItem>(specificAward), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeleteRankAward(int id, IRankAwardRepository rankAwardRepository)
    {
        return await rankAwardRepository.DeleteRankAwardByIdAsync(id) ? Results.Ok(ApiResponse.Success("Award is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find award with id = {id}"));
    }

    private static async Task<IResult> RemoveSpecificAward(int id, IRankAwardRepository rankAwardRepository)
    {
        return await rankAwardRepository.RemoveSpecificAwardAsync(id) ? Results.Ok(ApiResponse.Success("Specific award is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find specific award with id = {id}"));
    }

    private static async Task<IResult> SwitchPassed(short year, IRankAwardRepository rankAwardRepository)
    {
        return await rankAwardRepository.SwitchPassedStatusAsync(year) ? Results.Ok(ApiResponse.Success("Award is switched passed", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find rank award for year = {year}"));
    }

}
