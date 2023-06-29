using Microsoft.AspNetCore.Identity;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.AppUser;
using Stpm.Core.Entities;

namespace Stpm.Services.App;

public interface IUserRepository
{
    Task<IList<AppUserItem>> GetUsersAsync(CancellationToken cancellationToken = default);

    Task<UserTopicRating> GetUserTopicRatingAsync(int userId, int topicId, CancellationToken cancellationToken = default);

    Task<AppUserItem> GetUserItemByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<AppUserItem> GetCachedUserItemByIdAsync(int userId);

    Task<AppUser> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<AppUser> GetCachedUserByIdAsync(int userId);

    Task<AppUserItem> GetUserBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<AppUserItem> GetCachedUserBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IPagedList<AppUserItem>> GetUserByQueryAsync(AppUserQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    Task<IPagedList<AppUserItem>> GetUserByQueryAsync(AppUserQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetUserByQueryAsync<T>(AppUserQuery query, IPagingParams pagingParams, Func<IQueryable<AppUserItem>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateUserAsync(AppUser user, CancellationToken cancellationToken = default);

    Task<bool> SwitchUserStatusAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> CheckUserSlugExisted(int id, string slug, CancellationToken cancellationToken = default);

    Task<bool> SetImageUrlAsync(int userId, string imageUrl, CancellationToken cancellationToken = default);

    Task<TokenModel> LoginAsync(string usernameOrEmail, string password, bool rememberMe);

    Task<AuthModel> TokenLoginAsync(string token, string refreshToken);

    Task<AuthModel> ExternalLoginCallbackAsync(string returnUrl, string remoteError);

    Task<AuthModel> ConfirmExternalLoginAsync(string email, string returnUrl);

    Task<AuthModel> ConfirmEmailAsync(AppUser user, string code);

    Task<AuthModel> ConfirmEmailChangeAsync(AppUser user, string code, string email);

    Task<IList<AppUser>> AddUsersByRegisTempAsync(int topicId, string regisTemp, CancellationToken cancellationToken = default);

    Task<IList<AppUserRole>> GetRolesAsync(CancellationToken cancellationToken = default);

    Task<IList<IdentityRoleClaim<int>>> GetRoleClaimsAsync(int roleId, CancellationToken cancellationToken = default);

    Task<AppUserRole> GetRoleByIdAsync(int roleId, CancellationToken cancellationToken = default);

    Task<AppUserRole> GetCachedRoleByIdAsync(int roleId);

    Task<string> AddOrEditRoleAsync(AppUserRole role, CancellationToken cancellationToken = default);

    Task<string> AddOrEditUserRoleAsync(AppUser user, string[] roles, CancellationToken cancellationToken = default);

    Task<bool> DeleteRoleAsync(AppUserRole role, CancellationToken cancellationToken = default);

}
