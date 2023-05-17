using Carter;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stpm.Core.Collections;
using Stpm.Core.DTO.Comment;
using Stpm.Core.DTO.Post;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.Services.Media;
using Stpm.WebApi.Extensions;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.Comment;
using Stpm.WebApi.Models.Post;
using System.Net;
using System.Web;

namespace Stpm.WebApi.Endpoints;

public class PostEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/posts");

        // Nested Map with defined specific route
        routeGroupBuilder.MapGet("/", GetPosts)
                         .WithName("GetPosts");

        routeGroupBuilder.MapGet("/featured/{limit:int}", GetPopularPosts)
                         .WithName("GetPopularPosts");

        routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPost)
                         .WithName("GetRandomPost");

        routeGroupBuilder.MapGet("/{id:int}", GetPostById)
                         .WithName("GetPostById");

        routeGroupBuilder.MapGet("/byslug/{slug::regex(^[a-z0-9_-]+$)}", GetPostBySlug)
                         .WithName("GetPostBySlug");

        routeGroupBuilder.MapPost("/", AddOrUpdatePost)
                         .WithName("AddOrUpdatePost")
                         .Accepts<PostEditModel>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                         .WithName("DeletePost");

        routeGroupBuilder.MapPut("/published/switch/{id:int}", SwitchPublished)
                         .WithName("SwitchPublished");

        routeGroupBuilder.MapPut("/view/increase/{id:int}", IncreaseView)
                         .WithName("IncreaseView");

        routeGroupBuilder.MapPut("/{id:int}/picture", SetPostPicture)
                         .WithName("SetPostPicture")
                         .Accepts<IFormFile>("multipart/form-data");

        routeGroupBuilder.MapPut("/{id:int}/video", SetPostVideo)
                         .WithName("SetPostVideo")
                         .Accepts<IFormFile>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id:int}/picture", RemovePostPicture)
                         .WithName("RemovePostPicture");

        routeGroupBuilder.MapDelete("/{id:int}/video", RemovePostVideo)
                         .WithName("RemovePostVideo");

        routeGroupBuilder.MapGet("/get-filter", GetFilter)
                         .WithName("GetPostFilter");
    }

    private static async Task<IResult> GetPosts([AsParameters] PostFilterModel model, IPostRepository postRepository, IMapper mapper)
    {
        var postQuery = mapper.Map<PostQuery>(model);
        var postsList = await postRepository.GetPostByQueryAsync(postQuery, model, posts => posts.ProjectToType<PostDto>());

        var paginationResult = new PaginationResult<PostDto>(postsList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetPopularPosts(int limit, IPostRepository postRepository, IMapper mapper)
    {
        var posts = await postRepository.GetPopularPostsAsync(limit);

        return Results.Ok(ApiResponse.Success(mapper.Map<List<PostDto>>(posts)));
    }

    private static async Task<IResult> GetRandomPost(int limit, IPostRepository postRepository, IMapper mapper)
    {
        var posts = await postRepository.GetRandomPostAsync(limit);

        return Results.Ok(ApiResponse.Success(mapper.Map<List<PostDto>>(posts)));
    }

    private static async Task<IResult> GetPostById(int id, IPostRepository postRepository, IMapper mapper)
    {
        var post = await postRepository.GetCachedPostByIdAsync(id);

        return post == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}")) : Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post)));
    }

    private static async Task<IResult> GetPostBySlug(string slug, IPostRepository postRepository, IMapper mapper)
    {
        var post = await postRepository.GetCachedPostBySlugAsync(slug);

        return post == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có slug {slug}")) : Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post)));
    }

    private static async Task<IResult> AddOrUpdatePost(HttpContext context, IPostRepository postRepository, IMapper mapper, IMediaManager mediaManager)
    {
        var model = await PostEditModel.BindAsync(context);
        var slug = model.Title.GenerateSlug();

        if (await postRepository.IsPostSlugExistedAsync(model.Id, slug))
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{slug}' đã được sử dụng cho bài viết khác"));
        }

        var post = model.Id > 0 ? await postRepository.GetPostByIdAsync(model.Id) : null;

        if (post == null)
        {
            post = new Post() { PostedDate = DateTime.Now };
        }

        post.Title = model.Title;
        post.UserId = model.UserId;
        post.ShortDescription = model.ShortDescription;
        post.Description = model.Description;
        post.Meta = model.Meta;
        post.Published = model.Published;
        post.ModifiedDate = DateTime.Now;
        post.UrlSlug = slug;

        var resultAdd = await postRepository.AddOrUpdatePostAsync(post, model.GetSelectedTags());

        if (resultAdd)
        {
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            foreach (IFormFile file in model.ImageFiles)
            {
                if (file?.Length > 0)
                {
                    string uploadedPath = await mediaManager.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);
                    if (!string.IsNullOrWhiteSpace(uploadedPath))
                    {
                        var uriFile = hostname + uploadedPath;
                        var result = await postRepository.AddImageUrlAsync(post.Id, uriFile);
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
                        var result = await postRepository.AddVideoUrlAsync(post.Id, uriFile);
                        if (!result)
                        {
                            await mediaManager.DeleteFileAsync(uploadedPath);
                        }
                    }
                }
            }
        }

        return Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeletePost(int id, IPostRepository postRepository)
    {
        return await postRepository.DeletePostByIdAsync(id) ? Results.Ok(ApiResponse.Success("Post is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find post with id = {id}"));
    }

    private static async Task<IResult> SwitchPublished(int id, IPostRepository postRepository)
    {
        return await postRepository.ChangePostStatusAsync(id) ? Results.Ok(ApiResponse.Success("Post is switch published", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find post with id = {id}"));
    }

    private static async Task<IResult> IncreaseView(int id, IPostRepository postRepository)
    {
        await postRepository.IncreaseViewCountAsync(id);
        return Results.Ok(ApiResponse.Success("Post is switch published", HttpStatusCode.NoContent));
    }

    private static async Task<IResult> SetPostPicture(int id, HttpContext context, IPostRepository postRepository, IMediaManager mediaManager)
    {
        string newImagePath = string.Empty;
        var form = await context.Request.ReadFormAsync();

        var file = form.Files.FirstOrDefault();

        if (file?.Length > 0)
        {
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            var uploadPath = await mediaManager.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);

            if (string.IsNullOrWhiteSpace(uploadPath))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            newImagePath = hostname + uploadPath;

            var result = await postRepository.AddVideoUrlAsync(id, newImagePath);

            if (!result)
            {
                string decodeUrl = HttpUtility.UrlDecode(newImagePath);
                var uri = new Uri(decodeUrl);

                var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));

                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not add video for post with id = {id}"));
            }
        }

        return Results.Ok(ApiResponse.Success(newImagePath));
    }

    private static async Task<IResult> RemovePostPicture(int id, string url, IPostRepository postRepository, IMediaManager mediaManager)
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
            Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Could not remove picture for post with url = {decodeUrl}"));
        }

        var result = await postRepository.RemoveImageUrlAsync(id, url);

        return result ? Results.Ok(ApiResponse.Success("Image is removed", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find post with id = {id}"));
    }

    private static async Task<IResult> SetPostVideo(int id, HttpContext context, IPostRepository postRepository, IMediaManager mediaManager)
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

            var result = await postRepository.AddVideoUrlAsync(id, newVideoPath);

            if (!result)
            {
                string decodeUrl = HttpUtility.UrlDecode(newVideoPath);
                var uri = new Uri(decodeUrl);

                var resultDelete = await mediaManager.DeleteFileAsync(uri.AbsolutePath.Substring(1, uri.AbsolutePath.Length - 1));

                return Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not add video for post with id = {id}"));
            }
        }

        return Results.Ok(ApiResponse.Success(newVideoPath));
    }

    private static async Task<IResult> RemovePostVideo(int id, string url, IPostRepository postRepository, IMediaManager mediaManager)
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
            Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Could not remove video for post with url = {decodeUrl}"));
        }

        var result = await postRepository.RemoveVideoUrlAsync(id, url);

        return result ? Results.Ok(ApiResponse.Success("Video is removed", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find post with id = {id}"));
    }

    private static async Task<IResult> GetFilter(IUserRepository userRepository)
    {
        var model = new PostFilterModel()
        {
            UserList = (await userRepository.GetUsersAsync())
                        .Select(a => new SelectListItem()
                        {
                            Text = $"{a.FullName} - {a.Roles.FirstOrDefault()}",
                            Value = a.Id.ToString()
                        }),
        };

        return Results.Ok(ApiResponse.Success(model));
    }
}