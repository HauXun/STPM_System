using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.DTO.TopicRank;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;

namespace Stpm.Services.App;

public class TopicRankRepository : ITopicRankRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public TopicRankRepository(StpmDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<IList<TopicRankItem>> GetTopicRanksAsync(CancellationToken cancellationToken = default)
    {
        var topicRanks = _dbContext.TopicRanks.AsSplitQuery().AsEnumerable();
        var rankAwards = _dbContext.RankAwards.AsSplitQuery().AsEnumerable();

        var groupAward = rankAwards.GroupBy(a => new
        {
            a.TopicRankId
        });
        var groupTopic = _dbContext.Topics.AsSplitQuery()
                                        .AsEnumerable()
                                        .GroupBy(t => new
                                        {
                                            t.RegisDate.Year,
                                            t.TopicRankId
                                        })
                                        .Join(topicRanks,
                                        topic => topic.Key.TopicRankId,
                                        tpRank => tpRank.Id,
                                        (topic, tpRank) => new
                                        {
                                            topic.Key.TopicRankId,
                                            tpRank.RankName,
                                            topic.Key.Year,
                                            TopicCount = topic.Count()
                                        });

        var result = groupTopic.Join(groupAward,
                                    topicItem => topicItem.TopicRankId,
                                    award => award.Key.TopicRankId,
                                    (topicItem, award) => new TopicRankItem
                                    {
                                        Id = topicItem.TopicRankId,
                                        RankName = topicItem.RankName,
                                        Year = topicItem.Year,
                                        TopicCount = topicItem.TopicCount,
                                        RankAwardCount = award.Count()
                                    });

        return await Task.FromResult(result.ToList());
    }

    public async Task<TopicRank> GetTopicRankByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TopicRanks.AsSplitQuery()
                                          .Where(t => t.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TopicRank> GetCachedTopicRankByIdAsync(int topicRankId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"topicRank.by-id.{topicRankId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetTopicRankByIdAsync(topicRankId, cancellationToken);
            });
    }

    public async Task<bool> AddOrUpdateTopicRankAsync(TopicRank topicRank, CancellationToken cancellationToken = default)
    {
        if (topicRank.Id > 0)
        {
            _dbContext.Update(topicRank);
        }
        else
        {
            await _dbContext.AddAsync(topicRank, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteTopicRankByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var topicRank = await _dbContext.TopicRanks.FindAsync(id);

        if (topicRank is null) return false;

        _dbContext.TopicRanks.Remove(topicRank);
        var rowsCount = await _dbContext.SaveChangesAsync(cancellationToken);

        return rowsCount > 0;
    }
}
