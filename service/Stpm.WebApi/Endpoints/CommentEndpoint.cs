using Carter;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stpm.Core.Collections;
using Stpm.Core.DTO.Comment;
using Stpm.Core.Entities;
using Stpm.Services.App;
using Stpm.WebApi.Filters;
using Stpm.WebApi.Models;
using Stpm.WebApi.Models.AppUser;
using Stpm.WebApi.Models.Comment;
using System.Net;

namespace Stpm.WebApi.Endpoints;

public class CommentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup("/api/comments");

        // Nested Map with defined specific route
        routeGroupBuilder.MapGet("/", GetComments)
                         .WithName("GetComments");

        routeGroupBuilder.MapGet("/{id:int}", GetCommentById)
                         .WithName("GetCommentById");

        routeGroupBuilder.MapPost("/", AddOrUpdateComment)
                         .WithName("AddOrUpdateComment")
                         .Accepts<CommentEditModel>("multipart/form-data");

        routeGroupBuilder.MapDelete("/{id:int}", DeleteComment)
                         .WithName("DeleteComment");

        routeGroupBuilder.MapGet("/get-filter", GetFilter)
                         .WithName("GetCommentFilter");
    }

    private static async Task<IResult> GetComments([AsParameters] CommentFilterModel model, ICommentRepository commentRepository, IMapper mapper)
    {
        var commentQuery = mapper.Map<CommentQuery>(model);
        var commentList = await commentRepository.GetCommentByQueryAsync(commentQuery, model, comment => comment.ProjectToType<CommentDto>());

        var paginationResult = new PaginationResult<CommentDto>(commentList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetCommentById(int id, ICommentRepository commentRepository, IMapper mapper)
    {
        var comment = await commentRepository.GetCachedCommentByIdAsync(id);

        return Results.Ok(ApiResponse.Success(mapper.Map<CommentDto>(comment)));
    }

    private static async Task<IResult> AddOrUpdateComment(HttpContext context, ICommentRepository commentRepository, IMapper mapper)
    {
        var model = await CommentEditModel.BindAsync(context);

        var comment = model.Id > 0 ? await commentRepository.GetCommentByIdAsync(model.Id) : null;
        if (comment == null)
        {
            comment = new Comment() { Date = DateTime.Now };
        }
        comment.Content = model.Content;
        comment.ModifiedDate = DateTime.Now;
        comment.UserId = model.UserId;

        await commentRepository.AddOrUpdateCommentAsync(comment);

        return Results.Ok(ApiResponse.Success(mapper.Map<CommentDto>(comment), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeleteComment(int id, ICommentRepository commentRepository)
    {
        return await commentRepository.DeleteCommentByIdAsync(id) ? Results.Ok(ApiResponse.Success("Comment is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find comment with id = {id}"));
    }

    private static async Task<IResult> GetFilter(IUserRepository userRepository, ITopicRepository topicRepository, IPostRepository postRepository)
    {
        var model = new CommentFilterModel()
        {
            UserList = (await userRepository.GetUsersAsync())
                        .Select(a => new SelectListItem()
                        {
                            Text = $"{a.FullName} - {a.Roles.FirstOrDefault()}",
                            Value = a.Id.ToString()
                        }),

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
}
