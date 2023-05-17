using Stpm.Core.Contracts;
using Stpm.Core.DTO.Tag;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface ITagRepository
{
    Task<IList<Tag>> GetTagListAsync(CancellationToken cancellationToken = default);

    Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<Tag> GetCachedTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<Tag> GetTagByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Tag> GetCachedTagByIdAsync(int tagId, CancellationToken cancellationToken = default);

    Task<IPagedList<Tag>> GetTagByQueryAsync(TagQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<IPagedList<Tag>> GetTagByQueryAsync(TagQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetTagByQueryAsync<T>(TagQuery query, IPagingParams pagingParams, Func<IQueryable<Tag>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<IList<TagItem>> GetTagItemsAsync(CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateTagAsync(Tag tag, CancellationToken cancellationToken = default);

    Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> CheckTagSlugExisted(int id, string slug, CancellationToken cancellationToken = default);
}
