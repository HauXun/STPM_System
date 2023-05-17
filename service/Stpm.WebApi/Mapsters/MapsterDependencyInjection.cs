using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Stpm.Core.DTO.AppUser;
using Stpm.Core.Entities;
using Stpm.WebApi.Models.AppUser;

namespace Stpm.WebApi.Mapsters;

public static class MapsterDependencyInjection
{
	public static WebApplicationBuilder ConfigureMapster(this WebApplicationBuilder builder)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MapsterConfiguration).Assembly);

        builder.Services.AddSingleton(config);
        builder.Services.AddScoped<IMapper, ServiceMapper>();

        return builder;
	}
}
