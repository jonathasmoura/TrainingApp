using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TraininApp.Infrastructure.Authentication;

namespace TraininApp.API.Configuration;

public static class JwtAuthenticationExtensions
{
	public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var jwtSection = configuration.GetSection(JwtOptions.SectionName);
		services.AddOptions<JwtOptions>().Bind(jwtSection).Validate(options => Encoding.UTF8.GetByteCount(options.Key) >= 32, "Jwt:Key must contain at least 32 bytes.").ValidateOnStart();
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
		{
			var jwtOptions = jwtSection.Get<JwtOptions>() ?? new JwtOptions();
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
				ValidateIssuer = true,
				ValidIssuer = jwtOptions.Issuer,
				ValidateAudience = true,
				ValidAudience = jwtOptions.Audience,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};
			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{
					var token = context.Request.Query["access_token"];
					if (!string.IsNullOrEmpty(token) && context.HttpContext.Request.Path.StartsWithSegments("/hubs")) context.Token = token;
					return Task.CompletedTask;
				}
			};
		});
		return services;
	}
}
