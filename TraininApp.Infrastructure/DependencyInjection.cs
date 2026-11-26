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
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException("ConnectionStrings:DefaultConnection must be configured through User Secrets or an environment secret.");
		services.AddDbContext<AcademyDbContext>(options => options.UseSqlServer(connectionString));
		services.AddSecurity(configuration);
		services.AddDistributedMemoryCache();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<DatabaseSeeder>();
		services.AddScoped<IReadOnlyAcademyContext>(sp => sp.GetRequiredService<AcademyDbContext>());
		services.AddScoped<IMemberRepository, MemberRepository>(); services.AddScoped<IProfessionalRepository, ProfessionalRepository>(); services.AddScoped<IExerciseRepository, ExerciseRepository>(); services.AddScoped<IPlanRepository, PlanRepository>(); services.AddScoped<IEnrollmentRepository, EnrollmentRepository>(); services.AddScoped<IInstallmentRepository, InstallmentRepository>(); services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
		services.AddScoped<IEnrollmentDomainService, EnrollmentDomainService>(); services.AddScoped<IWorkoutPlanDomainService, WorkoutPlanDomainService>();
		services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>(); services.AddSingleton<IIdempotencyService, IdempotencyService>(); services.AddScoped<IPaymentNotifier, NullPaymentNotifier>();
		services.AddScoped<ITokenService, JwtTokenService>();
		services.AddScoped<IIdentityService, IdentityService>();
		services.AddScoped<IPasswordResetEmailSender, PasswordResetEmailSender>();
		services.AddScoped<IDispatcher, Dispatcher>();
		var application = typeof(TraininApp.Application.Features.Members.ListMembersQuery).Assembly;
		services.AddMediatRLike(application);
		services.AddValidatorsFromAssembly(application);
		services.AddScoped<ITimelineService, TimelineService>();
		return services;
	}
	private static IServiceCollection AddMediatRLike(this IServiceCollection services, System.Reflection.Assembly assembly)
	{
		foreach (var type in assembly.GetTypes().Where(t => t is { IsAbstract: false, IsInterface: false }))
			foreach (var contract in type.GetInterfaces().Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) || i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))) services.AddScoped(contract, type);
		return services;
	}
}

