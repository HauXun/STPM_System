using Carter;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stpm.Core.Entities;
using Stpm.Core.Settings;
using Stpm.Data.Contexts;
using Stpm.Data.Seeders;
using Stpm.Services.App;
using Stpm.Services.Media;
using System.Text;
using System.Text.Json.Serialization;

namespace Stpm.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
        IConfigurationSection appSettings = builder.Configuration.GetSection("AppSettings");
        var secretKey = appSettings["SecretKey"];
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

        builder.Services.AddOptions();
        builder.Services.AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddCookie()
                        .AddJwtBearer(x =>
                        {
                            x.RequireHttpsMetadata = false;
                            x.SaveToken = true;
                            x.TokenValidationParameters = new TokenValidationParameters
                            {
                                // Tự cấp token
                                ValidateIssuer = true,
                                ValidIssuer = appSettings["ValidIssuer"],
                                ValidateAudience = true,
                                ValidAudience = appSettings["ValidAudience"],

                                // Ký vào token
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                                ValidateLifetime = true,
                                ClockSkew = TimeSpan.Zero
                            };
                            x.Events = new JwtBearerEvents
                            {
                                OnAuthenticationFailed = context =>
                                {
                                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                    {
                                        context.Response.Headers.Add("Token-Expired", "true");
                                    }
                                    return Task.CompletedTask;
                                }
                            };
                        })
                        .AddGoogle(googleOptions =>
                        {
                            // Đọc thông tin Authentication:Google từ appsettings.json

                            // Thiết lập ClientID và ClientSecret để truy cập API google
                            googleOptions.ClientId = googleAuthNSection["ClientId"];
                            googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                            // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
                            googleOptions.CallbackPath = "/google-login";

                            //this function is get user google profile image
                            googleOptions.Scope.Add("profile");
                            googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                        });

        builder.Services.AddAuthorization();


        //Cookie Policy needed for External Auth
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
        });

        builder.Services.Configure<FormOptions>(options =>
        {
            options.MemoryBufferThreshold = Int32.MaxValue;
            options.ValueCountLimit = int.MaxValue; //default 1024
            options.ValueLengthLimit = int.MaxValue; //not recommended value
            options.MultipartBodyLengthLimit = long.MaxValue; //not recommended value
        });

        builder.Services.AddCarter();

        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        builder.Services.AddMemoryCache();

        builder.Services.AddDbContext<StpmDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), options =>
            {
                options.CommandTimeout(120);
            });
        }, ServiceLifetime.Transient);

        // Dang ky Identity
        builder.Services.AddIdentity<AppUser, AppUserRole>()
                        .AddEntityFrameworkStores<StpmDbContext>()
                        .AddDefaultTokenProviders()
                        .AddTokenProvider(TokenOptions.DefaultProvider, typeof(DataProtectorTokenProvider<AppUser>))
                        .AddTokenProvider(TokenOptions.DefaultEmailProvider, typeof(EmailTokenProvider<AppUser>))
                        .AddTokenProvider(TokenOptions.DefaultPhoneProvider, typeof(PhoneNumberTokenProvider<AppUser>))
                        .AddTokenProvider(TokenOptions.DefaultAuthenticatorProvider, typeof(AuthenticatorTokenProvider<AppUser>));

        // Truy cập IdentityOptions
        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Thiết lập về Password
            options.Password.RequireDigit = false; // Không bắt phải có số
            options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
            options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
            options.Password.RequireUppercase = false; // Không bắt buộc chữ in
            options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
            options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

            // Cấu hình Lockout - khóa user
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
            options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 3 lần thì khóa
            options.Lockout.AllowedForNewUsers = true;

            // Cấu hình về User.
            options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;  // Email là duy nhất

            // Cấu hình đăng nhập.
            options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
            options.SignIn.RequireConfirmedAccount = true;

        });

        builder.Services.AddTransient<SendMailService>();
        builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();
        builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITopicRepository, TopicRepository>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<INotiLevelRepository, NotiLevelRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IProjectTimelineRepository, ProjectTimelineRepository>();
        builder.Services.AddScoped<ITimelineRepository, TimelineRepository>();
        builder.Services.AddScoped<IRankAwardRepository, RankAwardRepository>();
        builder.Services.AddScoped<ITopicRankRepository, TopicRankRepository>();

        builder.Services.AddScoped<IDataSeeder, DataSeeder>();

        //builder.Logging.ClearProviders();

        return builder;
    }

    public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("StpmSystem", policyBuilder => policyBuilder.AllowAnyOrigin()
                                                                          .AllowAnyHeader()
                                                                          .AllowAnyMethod());
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureSwaggerOpenApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllersWithViews()
                        .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        return builder;
    }

    public static WebApplication SetupRequestPipeLine(this WebApplication app)
    {
        app.UseRouting();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("StpmSystem");

        app.MapCarter();

        return app;
    }

    // Thêm dữ liệu mẫu vào CSDL
    public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        try
        {
            scope.ServiceProvider.GetRequiredService<IDataSeeder>().Initialize();
        }
        catch (Exception ex)
        {
            scope.ServiceProvider.GetRequiredService<ILogger<Program>>().LogError(ex, "Could not insert data into database");
        }

        return app;
    }
}
