using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TraininApp.Application.Abstractions;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Services;
using TraininApp.Infrastructure.CQRS;
using TraininApp.Infrastructure.Authentication;
using TraininApp.Infrastructure.Persistence;
using TraininApp.Infrastructure.Services;

namespace TraininApp.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AcademyDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
		services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
		services.AddIdentityCore<ApplicationUser>(options =>
		{
			options.User.RequireUniqueEmail = true;
			options.Password.RequiredLength = 8;
			options.Password.RequireNonAlphanumeric = true;
		}).AddRoles<Microsoft.AspNetCore.Identity.IdentityRole<Guid>>().AddEntityFrameworkStores<AcademyDbContext>();
		services.AddDistributedMemoryCache();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IReadOnlyAcademyContext>(sp => sp.GetRequiredService<AcademyDbContext>());
		services.AddScoped<IMemberRepository, MemberRepository>(); services.AddScoped<IProfessionalRepository, ProfessionalRepository>(); services.AddScoped<IExerciseRepository, ExerciseRepository>(); services.AddScoped<IPlanRepository, PlanRepository>(); services.AddScoped<IEnrollmentRepository, EnrollmentRepository>(); services.AddScoped<IInstallmentRepository, InstallmentRepository>(); services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
		services.AddScoped<IEnrollmentDomainService, EnrollmentDomainService>(); services.AddScoped<IWorkoutPlanDomainService, WorkoutPlanDomainService>();
		services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>(); services.AddSingleton<IIdempotencyService, IdempotencyService>(); services.AddScoped<IPaymentNotifier, NullPaymentNotifier>();
		services.AddScoped<ITokenService, JwtTokenService>();
		services.AddScoped<IDispatcher, Dispatcher>();
		var application = typeof(TraininApp.Application.Features.Members.ListMembersQuery).Assembly;
		services.AddMediatRLike(application);
		services.AddValidatorsFromAssembly(application);
		return services;
	}
	private static IServiceCollection AddMediatRLike(this IServiceCollection services, System.Reflection.Assembly assembly)
	{
		foreach (var type in assembly.GetTypes().Where(t => t is { IsAbstract: false, IsInterface: false }))
			foreach (var contract in type.GetInterfaces().Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) || i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))) services.AddScoped(contract, type);
		return services;
	}
}

