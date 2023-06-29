﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.Tag;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using Stpm.Services.Extensions;

namespace Stpm.Services.App;

public class TagRepository : ITagRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public TagRepository(StpmDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public async Task<IList<Tag>> GetTagListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tags.AsNoTracking()
                                    .ToListAsync(cancellationToken);
    }

    public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tags.AsSplitQuery()
                                    .Where(t => t.UrlSlug.Equals(slug))
                                    .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Tag> GetCachedTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"tag.by-slug.{slug}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetTagBySlugAsync(slug, cancellationToken);
            });
    }

    public async Task<Tag> GetTagByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tags.FindAsync(id, cancellationToken);
    }

    public async Task<Tag> GetCachedTagByIdAsync(int tagId, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"tag.by-id.{tagId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetTagByIdAsync(tagId, cancellationToken);
            });
    }

    public async Task<IPagedList<Tag>> GetTagByQueryAsync(TagQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterTags(query).ToPagedListAsync(
                                pageNumber,
                                pageSize,
                                nameof(Tag.Name),
                                "DESC",
                                cancellationToken);
    }

    public async Task<IPagedList<Tag>> GetTagByQueryAsync(TagQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await FilterTags(query).ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetTagByQueryAsync<T>(TagQuery query, IPagingParams pagingParams, Func<IQueryable<Tag>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        IQueryable<T> result = mapper(FilterTags(query));

        return await result.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IList<TagItem>> GetTagItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Tag>()
                                .Select(x => new TagItem()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    UrlSlug = x.UrlSlug,
                                    Description = x.Description,
                                    PostCount = x.Posts.Count()
                                }).ToListAsync(cancellationToken);
    }

    public async Task<bool> AddOrUpdateTagAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        if (tag.Id > 0)
            _dbContext.Update(tag);
        else
            await _dbContext.AddAsync(tag, cancellationToken);

        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (_dbContext.Tags == null)
        {
            Console.WriteLine("Không có tag nào");
            return await Task.FromResult(false);
        }

        var tag = await _dbContext.Set<Tag>().FindAsync(id);

        if (tag != null)
        {
            Tag tagContext = tag;
            _dbContext.Tags.Remove(tagContext);

            Console.WriteLine($"Đã xóa tag với id {id}");
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<bool> CheckTagSlugExisted(int id, string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Tag>().AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
    }

    private IQueryable<Tag> FilterTags(TagQuery query)
    {
        IQueryable<Tag> categoryQuery = _dbContext.Tags
                                                .Include(c => c.Posts)
                                                .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            categoryQuery = categoryQuery.Where(x => x.Name.Contains(query.Name));
        }

        if (!string.IsNullOrWhiteSpace(query.PostSlug))
        {
            categoryQuery = categoryQuery.Where(x => x.Posts.Any(p => p.UrlSlug== query.PostSlug));
        }

        if (!string.IsNullOrWhiteSpace(query.UrlSlug))
        {
            categoryQuery = categoryQuery.Where(x => x.UrlSlug == query.UrlSlug);
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            categoryQuery = categoryQuery.Where(x => x.Name.Contains(query.Keyword) ||
                         x.Description.Contains(query.Keyword) ||
                         x.Posts.Any(p => p.Title.Contains(query.Keyword)));
        }

        return categoryQuery;
    }
}