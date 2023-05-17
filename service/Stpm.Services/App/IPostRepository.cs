using Stpm.Core.Contracts;
using Stpm.Core.DTO.Post;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface IPostRepository
{
    Task<IList<Post>> GetPostsAsync(CancellationToken cancellationToken = default);

    Task<Post> GetPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default);

    Task<Post> GetCachedPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default);

    Task<Post> GetPostBySlugAsync(string slug, bool published = false, CancellationToken cancellationToken = default);

    Task<Post> GetCachedPostBySlugAsync(string slug, bool published = false, CancellationToken cancellationToken = default);

    Task<IList<Post>> GetPopularPostsAsync(int limit, CancellationToken cancellationToken = default);

    Task<IList<Post>> GetRandomPostAsync(int limit, CancellationToken cancellationToken = default);

    Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetPostByQueryAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);

    Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default);

    Task<bool> IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default);

    Task<bool> ChangePostStatusAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> AddImageUrlAsync(int postId, string imageUrl, CancellationToken cancellationToken = default);

    Task<bool> AddVideoUrlAsync(int postId, string videoUrl, CancellationToken cancellationToken = default);

    Task<bool> RemoveImageUrlAsync(int postId, string imageUrl, CancellationToken cancellationToken = default);

    Task<bool> RemoveVideoUrlAsync(int postId, string videoUrl, CancellationToken cancellationToken = default);
}
