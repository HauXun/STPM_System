using Carter;
using Mapster;
using MapsterMapper;
using Stpm.Core.Collections;
using Stpm.Core.DTO.ProjectTimeline;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.ProjectTimeline;
using System.Net;

namespace Stpm.WebApi.Endpoints;

public class ProjectTimelineEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/pjtimelines");

        // Nested Map with defined specific route
        routeGroupBuilder.MapGet("/", GetProjectTimelines)
                 .WithName("GetProjectTimelines");

        routeGroupBuilder.MapGet("/{id:int}", GetProjectTimelineById)
                 .WithName("GetProjectTimelineById");

        routeGroupBuilder.MapPost("/", AddOrUpdateProjectTimeline)
                 .WithName("AddOrUpdateProjectTimeline")
                 .Accepts<ProjectTimelineEditModel>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id:int}", DeleteProjectTimeline)
                 .WithName("DeleteProjectTimeline");

        routeGroupBuilder.MapPut("/showon/switch/{id:int}", SwitchShowOn)
                 .WithName("SwitchShowOn");
    }

    private static async Task<IResult> GetProjectTimelines([AsParameters] ProjectTimelineFilterModel model, IProjectTimelineRepository projectTimelineRepository, IMapper mapper)
    {
        var projectTimelineQuery = mapper.Map<ProjectTimelineQuery>(model);
        var projectTimelinesList = await projectTimelineRepository.GetProjectTimelineByQueryAsync(projectTimelineQuery, model, projectTimelines => projectTimelines.ProjectToType<ProjectTimelineDto>());

        var paginationResult = new PaginationResult<ProjectTimelineDto>(projectTimelinesList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetProjectTimelineById(int id, IProjectTimelineRepository projectTimelineRepository, IMapper mapper)
    {
        var projectTimeline = await projectTimelineRepository.GetCachedProjectTimelineByIdAsync(id);

        return projectTimeline == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}")) : Results.Ok(ApiResponse.Success(mapper.Map<ProjectTimelineDto>(projectTimeline)));
    }

    private static async Task<IResult> AddOrUpdateProjectTimeline(HttpContext context, IProjectTimelineRepository projectTimelineRepository, IMapper mapper)
    {
        var model = await ProjectTimelineEditModel.BindAsync(context);

        var projectTimeline = model.Id > 0 ? await projectTimelineRepository.GetProjectTimelineByIdAsync(model.Id) : null;

        if (projectTimeline == null)
        {
            projectTimeline = new ProjectTimeline();
        }

        projectTimeline.Title = model.Title;
        projectTimeline.ShortDescription = model.ShortDescription;
        projectTimeline.ShowOn = model.ShowOn;

        await projectTimelineRepository.AddOrUpdateProjectTimelineAsync(projectTimeline);

        return Results.Ok(ApiResponse.Success(mapper.Map<ProjectTimelineItem>(projectTimeline), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeleteProjectTimeline(int id, IProjectTimelineRepository projectTimelineRepository)
    {
        return await projectTimelineRepository.DeleteProjectTimelineByIdAsync(id) ? Results.Ok(ApiResponse.Success("Project is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find project with id = {id}"));
    }

    private static async Task<IResult> SwitchShowOn(int id, IProjectTimelineRepository projectTimelineRepository)
    {
        return await projectTimelineRepository.SwitchProjectTimelineStatusAsync(id) ? Results.Ok(ApiResponse.Success("Project is switched show on", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find project with id = {id}"));
    }

}
