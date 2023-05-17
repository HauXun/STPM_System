using Stpm.Core.DTO.TopicRank;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface ITopicRankRepository
{
    Task<IList<TopicRankItem>> GetTopicRanksAsync(CancellationToken cancellationToken = default);

    Task<TopicRank> GetTopicRankByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TopicRank> GetCachedTopicRankByIdAsync(int topicRankId, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateTopicRankAsync(TopicRank topicRank, CancellationToken cancellationToken = default);

    Task<bool> DeleteTopicRankByIdAsync(int id, CancellationToken cancellationToken = default);
}
