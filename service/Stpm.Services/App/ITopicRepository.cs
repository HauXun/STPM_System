using Stpm.Core.Contracts;
using Stpm.Core.DTO.Topic;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface ITopicRepository
{
    Task<IList<Topic>> GetTopicsAsync(CancellationToken cancellationToken = default);

    Task<Topic> GetTopicByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Topic> GetCachedTopicByIdAsync(int topicId, CancellationToken cancellationToken = default);

    Task<Topic> GetTopicBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<Topic> GetCachedTopicBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IPagedList<Topic>> GetTopicByQueryAsync(TopicQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<IPagedList<Topic>> GetTopicByQueryAsync(TopicQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetTopicByQueryAsync<T>(TopicQuery query, IPagingParams pagingParams, Func<IQueryable<Topic>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateTopicAsync(Topic topic, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateTopicHistoryAsync(TopicHistoryAward topic, CancellationToken cancellationToken = default);

    Task<bool> SwitchRegisteredStatusAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> SwitchCancelStatusAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> SwitchForceLockStatusAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> SpecificTopicUserAsync(int userId, int topicId, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateUserSpecificMarkAsync(int userId, int topicId, float? mark = null, CancellationToken cancellationToken = default);

    Task<bool> UserRemoveSpecificMarkAsync(int userId, int topicId, CancellationToken cancellationToken = default);

    Task<bool> RemoveSpecificTopicUserAsync(int userId, int topicId, CancellationToken cancellationToken = default);

    Task<bool> AddImageUrlAsync(int topicId, string imageUrl, CancellationToken cancellationToken = default);

    Task<bool> SetOutlineUrlAsync(int topicId, string outlineUrl, CancellationToken cancellationToken = default);

    Task<bool> AddVideoUrlAsync(int topicId, string videoUrl, CancellationToken cancellationToken = default);

    Task<bool> RemoveImageUrlAsync(int topicId, string imageUrl, CancellationToken cancellationToken = default);

    Task<bool> RemoveVideoUrlAsync(int topicId, string videoUrl, CancellationToken cancellationToken = default);
}
