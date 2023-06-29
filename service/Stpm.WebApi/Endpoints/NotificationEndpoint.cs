using Carter;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
using Stpm.Core.Collections;
using Stpm.Core.DTO.Notification;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.Services.Media;
using Stpm.WebApi.Filters;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.Comment;
using Stpm.WebApi.Models.Notification;
using System.Net;
using System.Web;

namespace Stpm.WebApi.Endpoints;

public class NotificationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/notifications");

        routeGroupBuilder.MapGet("/", GetNotifications)
                         .WithName("GetNotifications");

        routeGroupBuilder.MapGet("/{id:int}/attachments", GetNotificationAttachments)
                         .WithName("GetNotificationAttachments");

        routeGroupBuilder.MapGet("/{id:int}", GetNotificationById)
                         .WithName("GetNotificationById");

        routeGroupBuilder.MapPost("/", AddOrUpdateNotification)
                         .WithName("AddOrUpdateNotification")
                         .Accepts<NotificationEditModel>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id:int}", DeleteNotificationt)
                         .WithName("DeleteNotificationt");

        routeGroupBuilder.MapPut("/{id:int}/status/switch/{userId:int}", SwitchUserNotifyStatus)
                         .WithName("SwitchUserNotifyStatus");

        routeGroupBuilder.MapPost("/{id:int}/specific/{userId:int}", AddNotificationForUser)
                         .WithName("AddNotificationForUser");

        routeGroupBuilder.MapPost("/{id:int}/specific/{timelineId:int}", AddNotificationForTimeline)
                         .WithName("AddNotificationForTimeline");

        routeGroupBuilder.MapPost("/{id:int}/attachment", AddAttachmentUrl)
                         .WithName("AddAttachmentUrl")
                         .Accepts<IFormFile>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id:int}/attachment/{url}", RemoveAttachmentUrl)
                         .WithName("RemoveAttachmentUrl");
    }

    private static async Task<IResult> GetNotifications([AsParameters] NotificationFilterModel model, INotificationRepository notificationRepository, IMapper mapper)
    {
        var notificationQuery = mapper.Map<NotificationQuery>(model);
        var notificationList = await notificationRepository.GetNotificationByQueryAsync(notificationQuery, model, notification => notification.ProjectToType<NotificationDto>());

        var paginationResult = new PaginationResult<NotificationDto>(notificationList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetNotificationAttachments(int id, INotificationRepository notificationRepository)
    {
        var attachmentList = await notificationRepository.GetNotifyAttachmentByIdAsync(id);

        return Results.Ok(ApiResponse.Success(attachmentList));
    }

    private static async Task<IResult> GetNotificationById(int id, INotificationRepository notificationRepository, IMapper mapper)
    {
        var notify = await notificationRepository.GetCachedNotificationByIdAsync(id);

        return Results.Ok(ApiResponse.Success(mapper.Map<NotificationDto>(notify)));
    }

    private static async Task<IResult> AddOrUpdateNotification(HttpContext context, INotificationRepository notificationRepository, IMapper mapper, IMediaManager mediaManager)
    {
        var model = await NotificationEditModel.BindAsync(context);

        var notification = model.Id > 0 ? await notificationRepository.GetNotificationByIdAsync(model.Id) : null;

        if (notification == null)
        {
            notification = new Notification();
        }

        notification.Title = model.Title;
        notification.Content = model.Content;
        notification.DueDate = model.DueDate;
        notification.LevelId = model.LevelId;

        var resultAdd = await notificationRepository.AddOrUpdateNotificationAsync(notification);

        if (resultAdd)
        {
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            foreach (IFormFile file in model.Files)
            {
                if (file?.Length > 0)
                {
                    string uploadedPath = await mediaManager.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType, MIMEType.Media);
                    if (!string.IsNullOrWhiteSpace(uploadedPath))
                    {
                        var uriFile = hostname + uploadedPath;
                        await notificationRepository.AddAttachmentUrlAsync(notification.Id, uriFile);
                    }
                }
            }
        }

        if (model.Users?.Length > 0)
        {
            foreach (var user in model.Users)
            {
                await notificationRepository.AddNotificationForUserAsync(user, notification.Id);
            }
        }

        return Results.Ok(ApiResponse.Success(mapper.Map<NotificationDto>(notification), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeleteNotificationt(int id, INotificationRepository notificationRepository)
    {
        return await notificationRepository.DeleteNotificationByIdAsync(id) ? Results.Ok(ApiResponse.Success("Notification is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find notification with id = {id}"));
    }

    private static async Task<IResult> SwitchUserNotifyStatus(int id, int userId, INotificationRepository notificationRepository)
    {
        return await notificationRepository.ChangeNotificationStatusAsync(userId, id) ? Results.Ok(ApiResponse.Success("Notification is switch status", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find notification with id = {id} for user with id = {userId}"));
    }

    private static async Task<IResult> AddNotificationForUser(int id, int userId, INotificationRepository notificationRepository)
    {
        return await notificationRepository.AddNotificationForUserAsync(userId, id) ? Results.Ok(ApiResponse.Success("Notification is added for user", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not add notification with id = {id} for user with id = {userId}"));
    }

    private static async Task<IResult> AddNotificationForTimeline(int id, int timelineId, INotificationRepository notificationRepository)
    {
        return await notificationRepository.AddNotificationForTimelineAsync(timelineId, id) ? Results.Ok(ApiResponse.Success("Notification is added for user", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not add notification with id = {id} for timeline with id = {timelineId}"));
    }

    private static async Task<IResult> AddAttachmentUrl(int id, HttpContext context, INotificationRepository notificationRepository, IMediaManager mediaManager)
    {
        string newUrl = string.Empty;
        var form = await context.Request.ReadFormAsync();

        var file = form.Files.FirstOrDefault();

        if (file?.Length > 0)
        {
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            string uploadedPath = await mediaManager.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType, MIMEType.Media);
            if (!string.IsNullOrWhiteSpace(uploadedPath))
            {
                newUrl = hostname + uploadedPath;

                var result = await notificationRepository.AddAttachmentUrlAsync(id, newUrl);

                if (!result)
                {
                    await mediaManager.DeleteFileAsync(uploadedPath);
                    Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not add attachment for notification with id = {id}"));
                }
            }
        }

        return Results.Ok(ApiResponse.Success(newUrl));
    }

    private static async Task<IResult> RemoveAttachmentUrl(int id, string url, INotificationRepository notificationRepository, IMediaManager mediaManager)
    {
        string decodeUrl = HttpUtility.UrlDecode(url);
        var uri = new Uri(decodeUrl);
        
        var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));

        if (!resultDelete)
        {
            Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Could not remove attachment for notification with url = {decodeUrl}"));
        }

        var resultRemove = await notificationRepository.RemoveAttachmentUrlAsync(id, decodeUrl);

        return resultRemove ? Results.Ok(ApiResponse.Success("Attachment is removed", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not remove attachment for notification with id = {id}"));
    }
}
