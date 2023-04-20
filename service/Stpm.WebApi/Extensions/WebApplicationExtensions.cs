using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stpm.Core.Entities;
using Stpm.Data.Contexts;
using Stpm.Data.Seeders;
using Stpm.Services.Media;
using System.Text.Json.Serialization;

namespace Stpm.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions();
        builder.Services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies(o => { });
        builder.Services.AddAuthorization();

        builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

        builder.Services.AddMemoryCache();

        builder.Services.AddDbContext<StpmDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddTransient<SendMailService>();
        builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();

        builder.Services.AddScoped<IDataSeeder, DataSeeder>();

        // Dang ky Identity
        builder.Services.AddIdentity<AppUser, AppUserRole>()
                        .AddEntityFrameworkStores<StpmDbContext>()
                        .AddDefaultTokenProviders();

        builder.Services.AddDefaultIdentity<AppUser>(options =>
                        options.SignIn.RequireConfirmedAccount = true)
                        .AddRoles<AppUserRole>()
                        .AddEntityFrameworkStores<StpmDbContext>();

        builder.Services.AddIdentityCore<AppUser>(o =>
        {
            o.Stores.MaxLengthForKeys = 128;
            o.SignIn.RequireConfirmedAccount = true;
        });

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
                "abcdđefghijklmnopqrstuvwxyzABCDĐEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
            options.User.RequireUniqueEmail = true;  // Email là duy nhất


            // Cấu hình đăng nhập.
            options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
            options.SignIn.RequireConfirmedAccount = true;

        });

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
        if (app.Environment.IsDevelopment())
        {
            //app.UseSwagger();
            //app.UseSwaggerUI();
        }

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("StpmSystem");

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
