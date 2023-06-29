using Carter;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stpm.Core.Collections;
using Stpm.Core.DTO.Topic;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.Services.Media;
using Stpm.WebApi.Extensions;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.Topic;
using System.Net;
using System.Web;

namespace Stpm.WebApi.Endpoints;

public class TopicEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/topics");

        // Nested Map with defined specific route
        routeGroupBuilder.MapGet("/", GetTopics)
                         .WithName("GetTopics");

        routeGroupBuilder.MapGet("/{id:int}", GetTopicById)
                         .WithName("GetTopicById");

        routeGroupBuilder.MapGet("/byslug/{slug::regex(^[a-z0-9_-]+$)}", GetTopicBySlug)
                         .WithName("GetTopicBySlug");

        routeGroupBuilder.MapPost("/", AddOrUpdateTopic)
                         .WithName("AddNewTopic")
                         .Accepts<TopicEditModel>("multipart/form-data");

        routeGroupBuilder.MapPost("/specific", SpecificTopicUser)
                         .WithName("SpecificTopicUser");

        routeGroupBuilder.MapDelete("/specific/remove", RemoveSpecificTopicUser)
                         .WithName("RemoveSpecificTopicUser");

        routeGroupBuilder.MapPost("/specific/mark", AddOrUpdateUserSpecificMarkAsync)
                         .WithName("AddOrUpdateUserSpecificMarkAsync");

        routeGroupBuilder.MapDelete("/specific/mark/remove", UserRemoveSpecificMark)
                         .WithName("UserRemoveSpecificMark");

        routeGroupBuilder.MapPut("/cancel/switch/{id:int}", SwitchCancelTopic)
                         .WithName("SwitchCancelTopic");

        routeGroupBuilder.MapPut("/forcelock/switch/{id:int}", SwitchForceLockTopic)
                         .WithName("SwitchForceLockTopic");

        routeGroupBuilder.MapPut("/registerd/switch/{id:int}", SwitchRegisteredTopic)
                         .WithName("SwitchRegisteredTopic");

        routeGroupBuilder.MapPost("/{id:int}/outline", SetTopicOutline)
                         .WithName("SetTopicOutline")
                         .Accepts<IFormFile>("multipart/form-data");

        routeGroupBuilder.MapPut("/{id:int}/picture", SetTopicPicture)
                         .WithName("SetTopicPicture")
                         .Accepts<IFormFile>("multipart/form-data");

        routeGroupBuilder.MapPut("/{id:int}/video", SetTopicVideo)
                         .WithName("SetTopicVideo")
                         .Accepts<IFormFile>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id:int}/picture", RemoveTopicPicture)
                         .WithName("RemoveTopicPicture");

        routeGroupBuilder.MapDelete("/{id:int}/video", RemoveTopicVideo)
                         .WithName("RemoveTopicVideo");

        routeGroupBuilder.MapGet("/get-filter", GetFilter)
                         .WithName("GetTopicFilter");
    }

    private static async Task<IResult> GetTopics([AsParameters] TopicFilterModel model, ITopicRepository topicRepository, IMapper mapper)
    {
        var topicQuery = mapper.Map<TopicQuery>(model);
        var topicsList = await topicRepository.GetTopicByQueryAsync(topicQuery, model, topics => topics.ProjectToType<TopicDto>());

        var paginationResult = new PaginationResult<TopicDto>(topicsList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetTopicById(int id, ITopicRepository topicRepository, IMapper mapper)
    {
        var topic = await topicRepository.GetCachedTopicByIdAsync(id);

        return topic == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy đề tài có mã số {id}")) : Results.Ok(ApiResponse.Success(mapper.Map<TopicDto>(topic)));
    }

    private static async Task<IResult> GetTopicBySlug(string slug, ITopicRepository topicRepository, IMapper mapper)
    {
        var topic = await topicRepository.GetCachedTopicBySlugAsync(slug);

        return topic == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy đề tài có slug {slug}")) : Results.Ok(ApiResponse.Success(mapper.Map<TopicDto>(topic)));
    }

    private static async Task<IResult> AddOrUpdateTopic(HttpContext context, ITopicRepository topicRepository, IMapper mapper, IMediaManager mediaManager)
    {
        var model = await TopicEditModel.BindAsync(context);
        var slug = model.TopicName.GenerateSlug();

        var topic = model.Id > 0 ? await topicRepository.GetTopicByIdAsync(model.Id) : null;

        if (topic == null)
        {
            topic = new Topic() { RegisDate = DateTime.Now };
        }

        topic.TopicName = model.TopicName;
        topic.ShortDescription = model.ShortDescription;
        topic.Description = model.Description;
        topic.ForceLock = model.ForceLock;
        topic.Cancel = model.Cancel;
        topic.Registered = model.Registered;
        topic.RegisTemp = model.RegisTemp;
        topic.TopicRankId = model.TopicRankId;
        topic.LeaderId = model.LeaderId;
        topic.SpecificAwardId = model.SpecificAwardId ?? null;
        topic.UrlSlug = slug;

        string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";

        if (model.OutlineFile?.Length > 0)
        {
            string uploadedPath = await mediaManager.SaveFileAsync(model.OutlineFile.OpenReadStream(), model.OutlineFile.FileName, model.OutlineFile.ContentType, MIMEType.Media);
            if (!string.IsNullOrWhiteSpace(uploadedPath))
            {
                if (!string.IsNullOrEmpty(topic.OutlineUrl))
                {
                    string decodeUrl = HttpUtility.UrlDecode(topic.OutlineUrl);
                    var uri = new Uri(decodeUrl);

                    var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));
                }

                topic.OutlineUrl = hostname + uploadedPath;
            }
        }

        var resultAdd = await topicRepository.AddOrUpdateTopicAsync(topic);

        if (!resultAdd)
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, $"Could not add topic"));
        }

        foreach (IFormFile file in model.ImageFiles)
        {
            if (file?.Length > 0)
            {
                string uploadedPath = await mediaManager.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);
                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    var uriFile = hostname + uploadedPath;
                    var result = await topicRepository.AddImageUrlAsync(topic.Id, uriFile);
                    if (!result)
                    {
                        await mediaManager.DeleteFileAsync(uploadedPath);
                    }
                }
            }
        }

        foreach (IFormFile file in model.VideoFiles)
        {
            if (file?.Length > 0)
            {
                string uploadedPath = await mediaManager.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType, MIMEType.Video);
                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    var uriFile = hostname + uploadedPath;
                    var result = await topicRepository.AddVideoUrlAsync(topic.Id, uriFile);
                    if (!result)
                    {
                        await mediaManager.DeleteFileAsync(uploadedPath);
                    }
                }
            }
        }

        if (model.Users?.Length > 0)
        {
            var oldUsers = topic.Users.Select(u => u.Id).ToArray();
            var deleteUsers = oldUsers.Where(u => !model.Users.Contains(u)).ToArray();
            var addUsers = model.Users.Where(u => !oldUsers.Contains(u)).ToArray();

            foreach (var user in deleteUsers)
            {
                await topicRepository.RemoveSpecificTopicUserAsync(user, topic.Id);
            }

            foreach (var user in addUsers)
            {
                await topicRepository.SpecificTopicUserAsync(user, topic.Id);
            }
        }

        if (model.UserRatings?.Length > 0)
        {
            var oldUsers = topic.UserTopicRatings.Select(u => u.UserId).ToArray();
            var deleteUsers = oldUsers.Where(u => !model.UserRatings.Contains(u)).ToArray();
            var addUsers = model.UserRatings.Where(u => !oldUsers.Contains(u)).ToArray();

            foreach (var user in deleteUsers)
            {
                await topicRepository.UserRemoveSpecificMarkAsync(user, topic.Id);
            }

            foreach (var user in addUsers)
            {
                await topicRepository.AddOrUpdateUserSpecificMarkAsync(user, topic.Id);
            }
        }

        return Results.Ok(ApiResponse.Success(mapper.Map<TopicDto>(topic), HttpStatusCode.Created));
    }

    private static async Task<IResult> SwitchCancelTopic(int id, ITopicRepository topicRepository)
    {
        return await topicRepository.SwitchCancelStatusAsync(id) ? Results.Ok(ApiResponse.Success("Topic is switch canceled status", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {id}"));
    }

    private static async Task<IResult> SwitchForceLockTopic(int id, ITopicRepository topicRepository)
    {
        return await topicRepository.SwitchForceLockStatusAsync(id) ? Results.Ok(ApiResponse.Success("Topic is switch force lock status", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {id}"));
    }

    private static async Task<IResult> SwitchRegisteredTopic(int id, ITopicRepository topicRepository)
    {
        return await topicRepository.SwitchRegisteredStatusAsync(id) ? Results.Ok(ApiResponse.Success("Topic is switch registered status", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {id}"));
    }

    private static async Task<IResult> SpecificTopicUser(int userId, int topicId, ITopicRepository topicRepository)
    {
        return await topicRepository.SpecificTopicUserAsync(userId, topicId) ? Results.Ok(ApiResponse.Success("Topic is specified user", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {topicId} and user with id = {userId}"));
    }

    private static async Task<IResult> AddOrUpdateUserSpecificMarkAsync(int userId, int topicId, float? mark, ITopicRepository topicRepository)
    {
        return await topicRepository.AddOrUpdateUserSpecificMarkAsync(userId, topicId, mark) ? Results.Ok(ApiResponse.Success("Topic is specified mark by user", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {topicId} and user with id = {userId}"));
    }

    private static async Task<IResult> RemoveSpecificTopicUser(int userId, int topicId, ITopicRepository topicRepository)
    {
        return await topicRepository.RemoveSpecificTopicUserAsync(userId, topicId) ? Results.Ok(ApiResponse.Success("Topic is removed specific user", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {topicId} and user with id = {userId}"));
    }

    private static async Task<IResult> UserRemoveSpecificMark(int userId, int topicId, ITopicRepository topicRepository)
    {
        return await topicRepository.UserRemoveSpecificMarkAsync(userId, topicId) ? Results.Ok(ApiResponse.Success("Topic is removed specific rating user", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {topicId} and user with id = {userId}"));
    }

    private static async Task<IResult> SetTopicOutline(int id, IFormFile formFile, HttpContext context, ITopicRepository topicRepository, IMediaManager mediaManager)
    {
        var topic = await topicRepository.GetTopicByIdAsync(id);

        if (topic == null)
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {id}"));
        }

        var newPath = string.Empty;
        if (formFile?.Length > 0)
        {
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            string uploadedPath = await mediaManager.SaveFileAsync(formFile.OpenReadStream(), formFile.FileName, formFile.ContentType, MIMEType.Media);
            if (!string.IsNullOrWhiteSpace(uploadedPath))
            {
                // Nếu lưu thành công, xóa tập tin hình ảnh cũ (nếu có)
                if (!string.IsNullOrEmpty(topic.OutlineUrl))
                {
                    string decodeUrl = HttpUtility.UrlDecode(topic.OutlineUrl);
                    var uri = new Uri(decodeUrl);

                    var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));
                }

                newPath = hostname + uploadedPath;
                await topicRepository.SetOutlineUrlAsync(id, newPath);
            }
        }

        return Results.Ok(ApiResponse.Success(newPath));
    }

    private static async Task<IResult> SetTopicPicture(int id, HttpContext context, ITopicRepository topicRepository, IMediaManager mediaManager)
    {
        string newImagePath = string.Empty;
        var form = await context.Request.ReadFormAsync();

        var file = form.Files.FirstOrDefault();

        // Nếu người dùng có upload hình ảnh minh họa cho đề tài
        if (file?.Length > 0)
        {
            // Thực hiện việc lưu tập tin vào thư mực uploads
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            var uploadPath = await mediaManager.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);

            if (string.IsNullOrWhiteSpace(uploadPath))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            newImagePath = hostname + uploadPath;

            var result = await topicRepository.AddImageUrlAsync(id, newImagePath);

            if (!result)
            {
                string decodeUrl = HttpUtility.UrlDecode(newImagePath);
                var uri = new Uri(decodeUrl);

                var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));

                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not add picture for topic with id = {id}"));
            }
        }

        return Results.Ok(ApiResponse.Success(newImagePath));
    }

    private static async Task<IResult> RemoveTopicPicture(int id, string url, ITopicRepository topicRepository, IMediaManager mediaManager)
    {
        if (string.IsNullOrEmpty(url))
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find any url to remove"));
        }

        string decodeUrl = HttpUtility.UrlDecode(url);
        var uri = new Uri(decodeUrl);

        var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));

        if (!resultDelete)
        {
            Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Could not remove picture for topic with url = {decodeUrl}"));
        }

        var result = await topicRepository.RemoveImageUrlAsync(id, url);

        return result ? Results.Ok(ApiResponse.Success("Image is removed", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {id}"));
    }

    private static async Task<IResult> SetTopicVideo(int id, HttpContext context, ITopicRepository topicRepository, IMediaManager mediaManager)
    {
        string newVideoPath = string.Empty;
        var form = await context.Request.ReadFormAsync();

        var file = form.Files.FirstOrDefault();

        if (file?.Length > 0)
        {
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            var uploadPath = await mediaManager.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType, MIMEType.Video);

            if (string.IsNullOrWhiteSpace(uploadPath))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            newVideoPath = hostname + uploadPath;

            var result = await topicRepository.AddVideoUrlAsync(id, newVideoPath);

            if (!result)
            {
                string decodeUrl = HttpUtility.UrlDecode(newVideoPath);
                var uri = new Uri(decodeUrl);

                var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));

                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not add video for topic with id = {id}"));
            }
        }

        return Results.Ok(ApiResponse.Success(newVideoPath));
    }

    private static async Task<IResult> RemoveTopicVideo(int id, string url, ITopicRepository topicRepository, IMediaManager mediaManager)
    {
        if (string.IsNullOrEmpty(url))
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find any url to remove"));
        }

        string decodeUrl = HttpUtility.UrlDecode(url);
        var uri = new Uri(decodeUrl);

        var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));

        if (!resultDelete)
        {
            Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Could not remove picture for topic with url = {decodeUrl}"));
        }

        var result = await topicRepository.RemoveVideoUrlAsync(id, url);

        return result ? Results.Ok(ApiResponse.Success("Video is removed", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find topic with id = {id}"));
    }

    private static async Task<IResult> GetFilter(IUserRepository userRepository, IRankAwardRepository rankAwardRepository, IMapper mapper)
    {
        var model = new TopicFilterModel()
        {
            UserList = (await userRepository.GetUsersAsync())
                        .Select(a => new SelectListItem()
                        {
                            Text = $"{a.FullName} - {a.Roles.FirstOrDefault()}",
                            Value = a.Id.ToString()
                        }),
            RankAwardList = (await rankAwardRepository.GetRankAwardsAsync())
                        .Select(a => new SelectListItem()
                        {
                            Text = $"{a.AwardName} - {a.TopicRank.RankName}",
                            Value = a.Id.ToString()
                        }),
        };

        return Results.Ok(ApiResponse.Success(model));
    }

}
