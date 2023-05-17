using Stpm.Core.Contracts;
using Stpm.Core.DTO.Comment;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface ICommentRepository
{
    Task<IList<Comment>> GetCommentsAsync(CancellationToken cancellationToken = default);

    Task<Comment> GetCommentByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Comment> GetCachedCommentByIdAsync(int commentId, CancellationToken cancellationToken = default);

    Task<IPagedList<Comment>> GetCommentByQueryAsync(CommentQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<IPagedList<Comment>> GetCommentByQueryAsync(CommentQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetCommentByQueryAsync<T>(CommentQuery query, IPagingParams pagingParams, Func<IQueryable<Comment>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default);

    Task<bool> DeleteCommentByIdAsync(int id, CancellationToken cancellationToken = default);
}
