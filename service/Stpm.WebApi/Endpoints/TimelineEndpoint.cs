using Carter;
using Mapster;
using MapsterMapper;
using Stpm.Core.Collections;
using Stpm.Core.DTO.Timeline;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.Timeline;
using System.Net;

namespace Stpm.WebApi.Endpoints;

public class TimelineEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/timelines");

        // Nested Map with defined specific route
        routeGroupBuilder.MapGet("/", GetTimelines)
                         .WithName("GetTimelines");

        routeGroupBuilder.MapGet("/{id:int}", GetTimelineById)
                         .WithName("GetTimelineById");

        routeGroupBuilder.MapPost("/", AddOrUpdateTimeline)
                         .WithName("AddOrUpdateTimeline")
                         .Accepts<TimelineEditModel>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id:int}", DeleteTimeline)
                         .WithName("DeleteTimeline");

        routeGroupBuilder.MapDelete("/{id:int}/notify/{notifyId:int}", RemoveNotificationForTimeline)
                         .WithName("RemoveNotificationForTimeline");
    }

    private static async Task<IResult> GetTimelines([AsParameters] TimelineFilterModel model, ITimelineRepository timelineRepository, IMapper mapper)
    {
        var timelineQuery = mapper.Map<TimelineQuery>(model);
        var timelinesList = await timelineRepository.GetTimelineByQueryAsync(timelineQuery, model, timelines => timelines.ProjectToType<TimelineDto>());

        var paginationResult = new PaginationResult<TimelineDto>(timelinesList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetTimelineById(int id, ITimelineRepository timelineRepository, IMapper mapper)
    {
        var timeline = await timelineRepository.GetCachedTimelineByIdAsync(id);

        return timeline == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy timeline có mã số {id}")) : Results.Ok(ApiResponse.Success(mapper.Map<TimelineDto>(timeline)));
    }

    private static async Task<IResult> AddOrUpdateTimeline(HttpContext context, ITimelineRepository timelineRepository, INotificationRepository notificationRepository, IMapper mapper)
    {
        var model = await TimelineEditModel.BindAsync(context);

        var timeline = model.Id > 0 ? await timelineRepository.GetTimelineByIdAsync(model.Id) : null;

        if (timeline == null)
        {
            timeline = new Timeline();
        }

        timeline.Title = model.Title;
        timeline.ShortDescription = model.ShortDescription;
        timeline.DueDate = model.DueDate;
        timeline.ProjectId = model.ProjectId;

        await timelineRepository.AddOrUpdateTimelineAsync(timeline);

        if (model.Notifies?.Length > 0)
        {
            foreach (var notify in model.Notifies)
            {
                await notificationRepository.AddNotificationForTimelineAsync(timeline.Id, notify);
            }
        }

        return Results.Ok(ApiResponse.Success(mapper.Map<TimelineItem>(timeline), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeleteTimeline(int id, ITimelineRepository timelineRepository)
    {
        return await timelineRepository.DeleteTimelineByIdAsync(id) ? Results.Ok(ApiResponse.Success("Timeline is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find timeline with id = {id}"));
    }

    private static async Task<IResult> RemoveNotificationForTimeline(int id, int notifyId, INotificationRepository notificationRepository)
    {
        return await notificationRepository.RemoveNotificationForTimelineAsync(id, notifyId) ? Results.Ok(ApiResponse.Success("Timeline is deleted the notify", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not delete notify with id = {notifyId} for timeline with id = {id}"));
    }
}
