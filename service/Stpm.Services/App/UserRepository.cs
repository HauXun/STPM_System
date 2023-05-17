using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stpm.Core.Constants.Role;
using Stpm.Core.Contracts;
using Stpm.Core.DTO.AppUser;
using Stpm.Core.Entities;
using Stpm.Core.Settings;
using Stpm.Data.Contexts;
using Stpm.Services.Extensions;
using Stpm.Services.Media;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace Stpm.Services.App;

public class UserRepository : IUserRepository
{
    private readonly StpmDbContext _dbContext;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppUserRole> _roleManager;
    private readonly SendMailService _sendMailService;
    private readonly AppSettings _appSettings;
    private readonly IConfiguration Configuration;
    private readonly ILogger<UserRepository> _logger;
    private readonly IMemoryCache _memoryCache;

    public UserRepository(StpmDbContext dbContext, IMemoryCache memoryCache, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, SendMailService sendMailService, IConfiguration configuration, IOptions<AppSettings> optionsMonitor, ILogger<UserRepository> logger, RoleManager<AppUserRole> roleManager)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
        _signInManager = signInManager;
        _userManager = userManager;
        _sendMailService = sendMailService;
        Configuration = configuration;
        _appSettings = optionsMonitor.Value;
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task<IList<AppUserItem>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = _userManager.Users.AsSplitQuery()
                                      .AsNoTracking();

        var userItems = users.Select(x => new AppUserItem()
        {
            Id = x.Id,
            FullName = x.FullName,
            UrlSlug = x.UrlSlug,
            UserName = x.UserName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            ImageUrl = x.ImageUrl,
            JoinedDate = x.JoinedDate,
            GradeName = x.GradeName,
            MSSV = x.MSSV,
            LockEnable = x.LockoutEnabled,
            PostCount = x.Posts.Count(p => p.Published),
            TopicCount = x.Topics.Count(p => p.Registered),
            NotifyCount = x.UserNotifies.Count(p => !p.Viewed),
            CommentCount = x.Comments.Count(),
            TopicRatingCount = x.UserTopicRatings.Count(),
            Roles = _userManager.GetRolesAsync(x).GetAwaiter().GetResult().ToArray()
        });

        return await userItems.ToListAsync(cancellationToken);
    }

    public async Task<AppUserItem> GetUserItemByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var x = await _userManager.Users.AsSplitQuery()
                                        .Where(u => u.Id == id)
                                        .FirstOrDefaultAsync(cancellationToken);

        var userItem = new AppUserItem()
        {
            Id = x.Id,
            FullName = x.FullName,
            UrlSlug = x.UrlSlug,
            UserName = x.UserName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            ImageUrl = x.ImageUrl,
            JoinedDate = x.JoinedDate,
            GradeName = x.GradeName,
            MSSV = x.MSSV,
            LockEnable = x.LockoutEnabled,
            PostCount = x.Posts.Count(p => p.Published),
            TopicCount = x.Topics.Count(p => p.Registered),
            NotifyCount = x.UserNotifies.Count(p => !p.Viewed),
            CommentCount = x.Comments.Count(),
            TopicRatingCount = x.UserTopicRatings.Count(),
            Roles = _userManager.GetRolesAsync(x).GetAwaiter().GetResult().ToArray()
        };

        return userItem;
    }

    public async Task<AppUserItem> GetCachedUserItemByIdAsync(int userId)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"user.by-id.{userId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetUserItemByIdAsync(userId);
            });
    }

    public async Task<AppUser> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _userManager.Users.AsSplitQuery()
                                       .Where(u => u.Id == id)
                                       .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AppUser> GetCachedUserByIdAsync(int userId)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"user.by-id.{userId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetUserByIdAsync(userId);
            });
    }

    public async Task<AppUserItem> GetUserBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var x = await _userManager.Users.AsSplitQuery()
                                        .Where(u => u.UrlSlug.Equals(slug))
                                        .FirstOrDefaultAsync(cancellationToken);
        
        var userItem = new AppUserItem()
        {
            Id = x.Id,
            FullName = x.FullName,
            UrlSlug = x.UrlSlug,
            UserName = x.UserName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            ImageUrl = x.ImageUrl,
            JoinedDate = x.JoinedDate,
            GradeName = x.GradeName,
            MSSV = x.MSSV,
            LockEnable = x.LockoutEnabled,
            PostCount = x.Posts.Count(p => p.Published),
            TopicCount = x.Topics.Count(p => p.Registered),
            NotifyCount = x.UserNotifies.Count(p => !p.Viewed),
            CommentCount = x.Comments.Count(),
            TopicRatingCount = x.UserTopicRatings.Count(),
            Roles = _userManager.GetRolesAsync(x).GetAwaiter().GetResult().ToArray()
        };

        return userItem;
    }

    public async Task<AppUserItem> GetCachedUserBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"user.by-slug.{slug}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetUserBySlugAsync(slug, cancellationToken);
            });
    }

    public async Task<IPagedList<AppUserItem>> GetUserByQueryAsync(AppUserQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await FilterUsers(query).ToPagedListAsync(
                                pageNumber,
                                pageSize,
                                nameof(AppUserQuery.FullName),
                                "DESC",
                                cancellationToken);
    }

    public async Task<IPagedList<AppUserItem>> GetUserByQueryAsync(AppUserQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await FilterUsers(query).ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetUserByQueryAsync<T>(AppUserQuery query, IPagingParams pagingParams, Func<IQueryable<AppUserItem>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
    {
        IQueryable<T> result = mapper(FilterUsers(query));

        return await result.ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> AddOrUpdateUserAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        IdentityResult result;
        if (user.Id > 0)
        {
            result = await _userManager.UpdateAsync(user);
        }
        else
        {
            result = await _userManager.CreateAsync(user, "123");
        }

        return result.Succeeded;
    }

    public async Task<bool> SwitchUserStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FindAsync(id);

        user.LockoutEnabled = !user.LockoutEnabled;

        _dbContext.Attach(user).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> CheckUserSlugExisted(int id, string slug, CancellationToken cancellationToken = default)
    {
        return await _userManager.Users.AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
    }

    public async Task<bool> SetImageUrlAsync(int userId, string imageUrl, CancellationToken cancellationToken = default)
    {
        return await _userManager.Users.Where(x => x.Id == userId)
                                    .ExecuteUpdateAsync(x =>
                                        x.SetProperty(a => a.ImageUrl, a => imageUrl),
                                        cancellationToken) > 0;
    }

    public async Task<IList<AppUser>> AddUsersByRegisTempAsync(int topicId, string regisTemp, CancellationToken cancellationToken = default)
    {
        var topic = await _dbContext.Topics.Where(x => x.Id == topicId).FirstOrDefaultAsync();
        var splitUsersTemp = regisTemp.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
        List<AppUser> result = new List<AppUser>();

        foreach (var userTemp in splitUsersTemp)
        {
            string[] userInfo = null;
            AppUser user = null;

            try
            {
                if (userTemp.Contains("*"))
                {
                    userInfo = userTemp.Split("*")[1].Split(",");

                    user = new AppUser
                    {
                        FullName = userInfo[0],
                        MSSV = userInfo[1],
                        UserName = userInfo[1],
                        GradeName = userInfo[2],
                    };

                    topic.Leader = user;
                }
                else
                {
                    userInfo = userTemp.Split(",");

                    user = new AppUser
                    {
                        FullName = userInfo[0],
                        MSSV = userInfo[1],
                        UserName = userInfo[1],
                        GradeName = userInfo[2],
                    };
                }

                var resultAdd = await _userManager.CreateAsync(user, StringExtensions.GenerateRandomPassword());
                if (resultAdd.Succeeded)
                {
                    var resultAddRole = await _userManager.AddToRoleAsync(user, RoleName.Examinee);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callBackUrl = new UriBuilder("https", "localhost", 7150, "confirm-email", $"?userId={user.Id}&code={code}").Uri.ToString();

                    MailContent mailContent = new MailContent
                    {
                        To = $"{user.MSSV}@dlu.edu.vn",
                        Subject = "Xác thực tài khoản email của bạn",
                        Body = $"<h1>Đăng k&yacute; t&agrave;i khoản</h1><p>Ch&agrave;o bạn đ&acirc;y l&agrave; tin nhắn tự động từ hệ th&ocirc;ng Stpm System Dlu, ch&uacute;c mừng bạn đ&atilde; tạo t&agrave;i khoản th&agrave;nh c&ocirc;ng</p><p>Đ&acirc;y l&agrave; th&ocirc;ng tin t&agrave;i khoản được tạo của bạn.</p><blockquote>TK: aduanhba</blockquote><blockquote>MK: 2!df@fk&amp;</blockquote><p>Để đảm bảo t&iacute;nh bảo mật vui l&ograve;ng x&aacute;c thực t&agrave;i khoản của bạn qua đường linh dưới đ&acirc;y:</p><p>&nbsp;<a href=\"{HtmlEncoder.Default.Encode(callBackUrl)}\">Bấm vào đây để xác nhận</a>.</p>"
                    };

                    await _sendMailService.SendEmailAsync(mailContent);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        // Redirect to confirm account page
                    }

                    result.Add(user);
                    topic.Users.Add(user);
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw;
            }
            catch (NullReferenceException)
            {

            }
            catch (Exception)
            {

            }
        }

        return result;
    }

    public async Task<TokenModel> LoginAsync(string usernameOrEmail, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(usernameOrEmail, password, rememberMe, false);
        AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);

        if (!result.Succeeded || user == null)
        {
            user = await _userManager.FindByEmailAsync(usernameOrEmail);
            if (user != null)
            {
                result = await _signInManager.PasswordSignInAsync(usernameOrEmail, password, rememberMe, false);
            }
        }

        if (!result.Succeeded)
        {
            return new TokenModel();
        }

        if (result.RequiresTwoFactor)
        {

        }

        if (result.IsLockedOut)
        {
        }

        var jwtToken = await GenerateToken(user);

        return jwtToken;
    }

    public async Task<AuthModel> TokenLoginAsync(string token, string refreshToken)
    {
        var tokenModel = new TokenModel
        {
            AccessToken = token,
            RefreshToken = refreshToken
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);

        // Validate token
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _appSettings.ValidIssuer,
            ValidateAudience = true,
            ValidAudience = _appSettings.ValidAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true
        };

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            var utcExpireDate = long.Parse(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

            var expireDate = ConvertUnixTimeToDatetime(utcExpireDate);

            if (expireDate > DateTime.UtcNow)
            {
                return new AuthModel
                {
                    Message = "Access token has not yet expired"
                };
                // Success = false
                // Error = "Access token has not yet expired"
            }
            else
            {
                return await RenewToken(tokenModel);
            }
        }
        catch (SecurityTokenExpiredException eex)
        {
            return await RenewToken(tokenModel);
        }
    }

    public async Task<AuthModel> ConfirmEmailAsync(AppUser user, string code)
    {
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);


        if (result.Succeeded)
        {
            return new AuthModel<AppUser>
            {
                Message = "Email đã được xác thực",
                Redirect = RedirectAction.RedirectToPage,
                RedirectPath = "/",
                Data = user
            };
        }
        else
        {
            return new AuthModel<AppUser>
            {
                Message = "Lỗi xác thực email",
                Redirect = RedirectAction.Page,
            };
        }
    }

    public async Task<AuthModel> ConfirmEmailChangeAsync(AppUser user, string code, string email)
    {
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        var oldEmail = user.Email;

        var result = await _userManager.ChangeEmailAsync(user, email, code);

        if (!result.Succeeded)
        {
            return new AuthModel<AppUser>
            {
                Message = "Lỗi đổi địa chỉ email.",
                Redirect = RedirectAction.Page,
            };
        }

        // In our UI email and user name are one and the same, so when we update the email
        // we need to update the user name.

        if (user.UserName == oldEmail)
        {
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                return new AuthModel<AppUser>
                {
                    Message = "Lỗi thay đổi username.",
                    Redirect = RedirectAction.Page,
                };
            }
        }

        await _signInManager.RefreshSignInAsync(user);
        return new AuthModel<AppUser>
        {
            Message = "Thank you for confirming your email change.",
            Redirect = RedirectAction.Page,
        };
    }

    public async Task<AuthModel> ExternalLoginCallbackAsync(string returnUrl, string remoteError)
    {
        if (!string.IsNullOrEmpty(remoteError))
        {
            return new AuthModel
            {
                Message = $"Lỗi từ dịch vụ ngoài: {remoteError}",
                Redirect = RedirectAction.RedirectToPage,
                RedirectPath = "/login",
                ReturnUrl = returnUrl
            };
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return new AuthModel
            {
                Message = "Không lấy được thông tin từ dịch vụ ngoài",
                Redirect = RedirectAction.RedirectToPage,
                RedirectPath = "/login",
                ReturnUrl = returnUrl
            };
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            var jwtToken = await GenerateToken(user);

            return new AuthModel<AppUser>
            {
                Redirect = RedirectAction.LocalRedirect,
                ReturnUrl = returnUrl,
                JwtToken = jwtToken,
                Data = user
            };
        }
        if (result.IsLockedOut)
        {
            return new AuthModel
            {
                Redirect = RedirectAction.RedirectToPage,
                RedirectPath = "/lockout",
            };
        }
        else
        {
            // If the user does not have an account, then ask the user to create an account.
            //ReturnUrl = returnUrl;
            string email = string.Empty;
            string providerName = info.ProviderDisplayName;
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }

            return new AuthModel<Dictionary<string, string>>
            {
                Redirect = RedirectAction.Page,
                Data = new Dictionary<string, string>(){
                    {"providerName", providerName},
                    {"email", email},
                }
            };
        }
    }

    public async Task<AuthModel> ConfirmExternalLoginAsync(string email, string returnUrl)
    {
        List<string> errors = new List<string>();

        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return new AuthModel
            {
                Message = "Lỗi lấy thông tin từ dịch vụ ngoài",
                Redirect = RedirectAction.RedirectToPage,
                RedirectPath = "./login",
                ReturnUrl = returnUrl
            };
        }

        var registeredUser = await _userManager.FindByEmailAsync(email);
        string externalEmail = null;
        AppUser externalEmailUser = null;

        // Claim ~ Dac tinh mo ta mot doi tuong
        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        {
            externalEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
        }

        if (externalEmail != null)
        {
            externalEmailUser = await _userManager.FindByEmailAsync(externalEmail);
        }

        if ((registeredUser != null) && (externalEmailUser != null))
        {
            if (registeredUser.Id == externalEmailUser.Id)
            {
                var resultLink = await _userManager.AddLoginAsync(registeredUser, info);
                if (resultLink.Succeeded)
                {
                    return new AuthModel<AppUser>
                    {
                        Redirect = RedirectAction.LocalRedirect,
                        ReturnUrl = returnUrl,
                        Data = registeredUser
                    };
                }
            }
            else
            {
                return new AuthModel
                {
                    Message = "Không liên kết được tài khoản, hãy sử dụng tài khoản khác",
                    Redirect = RedirectAction.Page,
                    ReturnUrl = returnUrl,
                };
            }
        }

        if ((registeredUser == null) && (externalEmailUser != null))
        {
            return new AuthModel
            {
                Message = "Không hỗ trợ tạo tài khoản mới - có email khác email từ dịch vụ ngoài",
                Redirect = RedirectAction.Page,
                ReturnUrl = returnUrl,
            };
        }

        if ((externalEmailUser == null) && (externalEmail == email))
        {
            string fullName = externalEmail.Split("@")[0];
            var newUser = new AppUser
            {
                UserName = externalEmail,
                Email = externalEmail,
                FullName = fullName,
                UrlSlug = fullName.GenerateSlug(),
                JoinedDate = DateTime.Now
            };

            var resultNewUser = await _userManager.CreateAsync(newUser);
            if (resultNewUser.Succeeded)
            {
                await _userManager.AddLoginAsync(newUser, info);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                await _userManager.ConfirmEmailAsync(newUser, code);

                return new AuthModel<AppUser>
                {
                    Redirect = RedirectAction.LocalRedirect,
                    ReturnUrl = returnUrl,
                    Data = newUser
                };
            }
            else
            {
                return new AuthModel
                {
                    Message = "Không tạo được tài khoản mới",
                    Redirect = RedirectAction.Page,
                    ReturnUrl = returnUrl,
                };
            }
        }

        var user = CreateUser();

        await _userManager.SetUserNameAsync(user, email);
        await _userManager.SetEmailAsync(user, email);

        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            result = await _userManager.AddLoginAsync(user, info);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callBackUrl = new UriBuilder("https", "localhost", 7150, "confirm-email", $"?userId={user.Id}&code={code}").Uri.ToString();

                MailContent mailContent = new MailContent
                {
                    To = $"{email}",
                    Subject = "Xác thực tài khoản email của bạn",
                    Body = $"<h1>Đăng k&yacute; t&agrave;i khoản</h1><p>Ch&agrave;o bạn đ&acirc;y l&agrave; tin nhắn tự động từ hệ th&ocirc;ng Stpm System Dlu, ch&uacute;c mừng bạn đ&atilde; tạo t&agrave;i khoản th&agrave;nh c&ocirc;ng</p><p>Đ&acirc;y l&agrave; th&ocirc;ng tin t&agrave;i khoản được tạo của bạn.</p><blockquote>TK: aduanhba</blockquote><blockquote>MK: 2!df@fk&amp;</blockquote><p>Để đảm bảo t&iacute;nh bảo mật vui l&ograve;ng x&aacute;c thực t&agrave;i khoản của bạn qua đường linh dưới đ&acirc;y:</p><p>&nbsp;<a href=\"{HtmlEncoder.Default.Encode(callBackUrl)}\">Bấm vào đây để xác nhận</a>.</p>"
                };

                await _sendMailService.SendEmailAsync(mailContent);

                // If account confirmation is required, we need to show the link if we don't have a real email sender
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    //return RedirectToPage("./RegisterConfirmation", new { Email = email });
                    return new AuthModel
                    {
                        Redirect = RedirectAction.RedirectToPage,
                        RedirectPath = $"./register-confirmation?email={email}",
                    };
                }

                var jwtToken = await GenerateToken(user);

                return new AuthModel<AppUser>
                {
                    Redirect = RedirectAction.LocalRedirect,
                    ReturnUrl = returnUrl,
                    JwtToken = jwtToken,
                    Data = user
                };
            }

            errors = result.Errors.Any() ? result.Errors.Select(e => e.Description).ToList() : null;
        }

        string providerName = info.ProviderDisplayName;

        return new AuthModel<Dictionary<string, string>>
        {
            Message = string.Join("\r\n", errors) ?? "",
            Redirect = RedirectAction.Page,
            Data = new Dictionary<string, string>()
            {
                {"providerName", providerName},
                {"email", email},
            }
        };
    }

    private async Task<TokenModel> GenerateToken(AppUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

        // Get valid claims and pass them into JWT
        var claims = await GetValidClaims(user);

        // Create the JWT security token and encode it.
        //var tokenDescriptor = new SecurityTokenDescriptor
        //{
        //    Subject = new ClaimsIdentity(claims),
        //    Audience = _appSettings.ValidAudience,
        //    Issuer = _appSettings.ValidIssuer,
        //    Expires = DateTime.UtcNow.AddSeconds(30),
        //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
        //};
        var jwt = new JwtSecurityToken(
            issuer: _appSettings.ValidIssuer,
            audience: _appSettings.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(30),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature));

        //var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var accessToken = jwtTokenHandler.WriteToken(jwt);
        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            JwtId = jwt.Id,
            Token = refreshToken,
            UserId = user.Id,
            IsUsed = false,
            IsRevoked = false,
            IssuedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddSeconds(30)
        };

        await _dbContext.AddAsync(refreshTokenEntity);
        await _dbContext.SaveChangesAsync();

        return new TokenModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private async Task<List<Claim>> GetValidClaims(AppUser user)
    {
        IdentityOptions _options = new IdentityOptions();
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
            new Claim(_options.ClaimsIdentity.UserNameClaimType, user.UserName),
            new Claim(ClaimTypes.Name, user.FullName),
        };
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);
        claims.AddRange(userClaims);
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
            var role = await _userManager.FindByNameAsync(userRole);
            if (role != null)
            {
                var roleClaims = await _userManager.GetClaimsAsync(role);
                foreach (Claim roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }
        }
        return claims;
    }

    private string GenerateRefreshToken()
    {
        var random = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(random);

            return Convert.ToBase64String(random);
        }
    }

    private async Task<AuthModel> RenewToken(TokenModel tokenModel)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
        var tokenValidationParam = new TokenValidationParameters
        {
            // Tự cấp token
            ValidateIssuer = false,
            ValidateAudience = false,

            // Ký vào token
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

            // Kiểm tra hết hạn
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken, tokenValidationParam, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase);

                if (!result)
                {
                    return new AuthModel
                    {
                        Message = "Invalid token"
                    };
                    // Success = false
                    // Error = "Invalid token"
                }
            }

            var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

            var expireDate = ConvertUnixTimeToDatetime(utcExpireDate);

            if (expireDate > DateTime.UtcNow)
            {
                return new AuthModel
                {
                    Message = "Access token has not yet expired"
                };
                // Success = false
                // Error = "Access token has not yet expired"
            }

            var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenModel.RefreshToken);
            if (storedToken == null)
            {
                return new AuthModel
                {
                    Message = "Refresh token does not exist"
                };
                // Success = false
                // Error = "Refresh token does not exist"
            }

            if (storedToken.IsUsed)
            {
                return new AuthModel
                {
                    Message = "Refresh token has been used"
                };
                // Success = false
                // Error = "Refresh token has been used"
            }

            if (storedToken.IsRevoked)
            {
                return new AuthModel
                {
                    Message = "Refresh token has been revoked"
                };
                // Success = false
                // Error = "Refresh token has been revoked"
            }

            var jti = tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            if (storedToken.JwtId != jti)
            {
                return new AuthModel
                {
                    Message = "Token doesn't match"
                };
                // Success = false
                // Error = "Token doesn't match"
            }

            var user = await GetUserByIdAsync(storedToken.UserId);
            var token = await GenerateToken(user);

            storedToken.IsRevoked = true;
            storedToken.IsUsed = true;
            _dbContext.Update(storedToken);
            await _dbContext.SaveChangesAsync();

            return new AuthModel
            {
                JwtToken = token
            };
        }
        catch (Exception)
        {
            return new AuthModel
            {
                Message = "Something went wrong"
            };
            // bad request
            // Success = false
            // Error = "Something went wrong"
        }
    }

    private DateTime ConvertUnixTimeToDatetime(long utcExpireDate)
    {
        return DateTimeOffset.FromUnixTimeSeconds(utcExpireDate).UtcDateTime;
    }

    private AppUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<AppUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor");
        }
    }

    private IQueryable<AppUserItem> FilterUsers(AppUserQuery query)
    {
        IQueryable<AppUser> userQuery = _dbContext.Users.AsSplitQuery().AsNoTracking();

        if (query?.CommentId > 0)
        {
            userQuery = userQuery.Where(x => x.Comments.Any(c => c.Id == query.CommentId));
        }

        if (query?.TopicId > 0)
        {
            userQuery = userQuery.Where(x => x.Topics.Any(c => c.Id == query.TopicId));
        }

        if (query?.PostId > 0)
        {
            userQuery = userQuery.Where(x => x.Posts.Any(c => c.Id == query.PostId));
        }

        if (!string.IsNullOrWhiteSpace(query.FullName))
        {
            userQuery = userQuery.Where(x => x.FullName.Contains(query.FullName));
        }

        if (!string.IsNullOrWhiteSpace(query.PhoneNumber))
        {
            userQuery = userQuery.Where(x => x.PhoneNumber.Contains(query.PhoneNumber));
        }

        if (!string.IsNullOrWhiteSpace(query.UrlSlug))
        {
            userQuery = userQuery.Where(x => x.UrlSlug == query.UrlSlug);
        }

        if (!string.IsNullOrWhiteSpace(query.PostSlug))
        {
            userQuery = userQuery.Where(x => x.Posts.Any(p => p.UrlSlug == query.PostSlug));
        }

        if (!string.IsNullOrWhiteSpace(query.TopicSlug))
        {
            userQuery = userQuery.Where(x => x.Topics.Any(p => p.UrlSlug == query.TopicSlug));
        }

        if (!string.IsNullOrWhiteSpace(query.Email))
        {
            userQuery = userQuery.Where(x => x.Email.Contains(query.Email));
        }

        if (!string.IsNullOrWhiteSpace(query.MSSV))
        {
            userQuery = userQuery.Where(x => x.MSSV.Contains(query.MSSV));
        }

        if (!string.IsNullOrWhiteSpace(query.GradeName))
        {
            userQuery = userQuery.Where(x => x.GradeName.Contains(query.GradeName));
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            userQuery = userQuery.Where(x => x.FullName.Contains(query.Keyword) ||
                                             x.PhoneNumber.Contains(query.Keyword) ||
                                             x.Email.Contains(query.Keyword) ||
                                             x.MSSV.Contains(query.Keyword) ||
                                             x.GradeName.Contains(query.Keyword));
        }

        return userQuery.Select(x => new AppUserItem()
        {
            Id = x.Id,
            FullName = x.FullName,
            UrlSlug = x.UrlSlug,
            UserName = x.UserName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            ImageUrl = x.ImageUrl,
            JoinedDate = x.JoinedDate,
            GradeName = x.GradeName,
            MSSV = x.MSSV,
            LockEnable = x.LockoutEnabled,
            PostCount = x.Posts.Count(p => p.Published),
            TopicCount = x.Topics.Count(p => p.Registered),
            NotifyCount = x.UserNotifies.Count(p => !p.Viewed),
            CommentCount = x.Comments.Count(),
            TopicRatingCount = x.UserTopicRatings.Count(),
            Roles = _userManager.GetRolesAsync(x).GetAwaiter().GetResult().ToArray()
        });
    }

    public async Task<string> AddOrEditRoleAsync(AppUserRole role, CancellationToken cancellationToken = default)
    {
        IdentityResult result;
        if (role.Id > 0)
        {
            result = await _roleManager.UpdateAsync(role);
        }
        else
        {
            result = await _roleManager.CreateAsync(role);
        }

        return result.Succeeded ? "" : string.Join("\r\n", result.Errors.Select(e => e.Description).ToList());
    }

    public async Task<bool> DeleteRoleAsync(AppUserRole role, CancellationToken cancellationToken = default)
    {
        //if (roleId == null) return NotFound("Không tìm thấy role");

        //IdentityRole role = await _roleManager.FindByIdAsync(roleId);

        //if (role == null) return NotFound("Không tìm thấy role");

        var result = await _roleManager.DeleteAsync(role);
        return result.Succeeded;
    }

    public async Task<IList<AppUserRole>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
    }

    public async Task<IList<IdentityRoleClaim<int>>> GetRoleClaimsAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.RoleClaims.Where(c => c.RoleId == roleId).ToListAsync();
    }

    public async Task<AppUserRole> GetRoleByIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Roles.Where(r => r.Id == roleId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AppUserRole> GetCachedRoleByIdAsync(int roleId)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"userRole.by-id.{roleId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetRoleByIdAsync(roleId);
            });
    }

    public async Task<string> AddOrEditUserRoleAsync(AppUser user, string[] roles, CancellationToken cancellationToken = default)
    {
        //await GetClaims(id);

        var oldRoleNames = (await _userManager.GetRolesAsync(user)).ToArray();
        var deleteRoles = oldRoleNames.Where(r => !roles.Contains(r));
        var addRoles = roles.Where(r => !oldRoleNames.Contains(r));

        List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

        var resultDelete = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
        if (!resultDelete.Succeeded)
        {
            string.Join("\r\n", resultDelete.Errors.Select(e => e.Description).ToList());
        }

        var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
        if (!resultAdd.Succeeded)
        {
            string.Join("\r\n", resultAdd.Errors.Select(e => e.Description).ToList());
        }

        return string.Empty;
    }

}

public class AppIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError ConcurrencyFailure()
    {
        return base.ConcurrencyFailure();
    }

    public override IdentityError DefaultError()
    {
        return base.DefaultError();
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return base.DuplicateEmail(email);
    }

    public override IdentityError DuplicateRoleName(string role)
    {
        var er = base.DuplicateRoleName(role);

        return new IdentityError
        {
            Code = er.Code,
            Description = $"Role có tên {role} bị trùng"
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return base.DuplicateUserName(userName);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override IdentityError InvalidEmail(string email)
    {
        return base.InvalidEmail(email);
    }

    public override IdentityError InvalidRoleName(string role)
    {
        return base.InvalidRoleName(role);
    }

    public override IdentityError InvalidToken()
    {
        return base.InvalidToken();
    }

    public override IdentityError InvalidUserName(string userName)
    {
        return base.InvalidUserName(userName);
    }

    public override IdentityError LoginAlreadyAssociated()
    {
        return base.LoginAlreadyAssociated();
    }

    public override IdentityError PasswordMismatch()
    {
        return base.PasswordMismatch();
    }

    public override IdentityError PasswordRequiresDigit()
    {
        return base.PasswordRequiresDigit();
    }

    public override IdentityError PasswordRequiresLower()
    {
        return base.PasswordRequiresLower();
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return base.PasswordRequiresNonAlphanumeric();
    }

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
    {
        return base.PasswordRequiresUniqueChars(uniqueChars);
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return base.PasswordRequiresUpper();
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return base.PasswordTooShort(length);
    }

    public override IdentityError RecoveryCodeRedemptionFailed()
    {
        return base.RecoveryCodeRedemptionFailed();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override IdentityError UserAlreadyHasPassword()
    {
        return base.UserAlreadyHasPassword();
    }

    public override IdentityError UserAlreadyInRole(string role)
    {
        return base.UserAlreadyInRole(role);
    }

    public override IdentityError UserLockoutNotEnabled()
    {
        return base.UserLockoutNotEnabled();
    }

    public override IdentityError UserNotInRole(string role)
    {
        return base.UserNotInRole(role);
    }
}