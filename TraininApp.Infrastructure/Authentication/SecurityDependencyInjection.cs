using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TraininApp.Application.Interfaces;
using TraininApp.Infrastructure.Persistence;

namespace TraininApp.Infrastructure.Authentication;

public static class SecurityDependencyInjection
{
	public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentityCore<ApplicationUser>(options =>
		{
			options.User.RequireUniqueEmail = true;
			options.SignIn.RequireConfirmedEmail = false;
			options.Password.RequiredLength = 12;
			options.Password.RequireDigit = true;
			options.Password.RequireLowercase = true;
			options.Password.RequireUppercase = true;
			options.Password.RequireNonAlphanumeric = true;
			options.Lockout.MaxFailedAccessAttempts = 5;
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
		})
		.AddRoles<IdentityRole<Guid>>()
		.AddEntityFrameworkStores<AcademyDbContext>()
		.AddSignInManager()
		.AddDefaultTokenProviders();

		services.Configure<PasswordHasherOptions>(options =>
		{
			options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
			options.IterationCount = 310_000;
		});
		services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
		services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(1));
		services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
		services.AddOptions<JwtOptions>()
			.Bind(configuration.GetSection(JwtOptions.SectionName))
			.Validate(options => Encoding.UTF8.GetByteCount(options.Key) >= 32, "Jwt:Key must contain at least 32 bytes.")
			.Validate(options => !string.IsNullOrWhiteSpace(options.Issuer), "Jwt:Issuer is required.")
			.Validate(options => !string.IsNullOrWhiteSpace(options.Audience), "Jwt:Audience is required.")
			.Validate(options => options.ExpirationMinutes is >= 5 and <= 60, "Jwt:ExpirationMinutes must be between 5 and 60.")
			.ValidateOnStart();

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options => ConfigureJwtBearer(options, configuration));
		services.AddAuthorization();
		return services;
	}

	private static void ConfigureJwtBearer(JwtBearerOptions options, IConfiguration configuration)
	{
		options.MapInboundClaims = false;
		var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
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
			},
			OnTokenValidated = async context =>
			{
				var tokenId = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
				if (tokenId is null) return;
				var blacklist = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
				if (await blacklist.IsRevokedAsync(tokenId, context.HttpContext.RequestAborted)) context.Fail("Token revoked.");
			}
		};
	}
}
