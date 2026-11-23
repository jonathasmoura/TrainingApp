using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TraininApp.Application.Interfaces;
using TraininApp.Domain.Entities;

namespace TraininApp.Infrastructure.Persistence;

public sealed class AcademyDbContext(DbContextOptions<AcademyDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IReadOnlyAcademyContext, IUnitOfWork
{
	public DbSet<Member> Members => Set<Member>();
	public DbSet<Professional> Professionals => Set<Professional>();
	public DbSet<Exercise> Exercises => Set<Exercise>();
	public DbSet<Plan> Plans => Set<Plan>();
	public DbSet<Enrollment> Enrollments => Set<Enrollment>();
	public DbSet<Installment> Installments => Set<Installment>();
	public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
	public DbSet<WorkoutItem> WorkoutItems => Set<WorkoutItem>();
	IQueryable<Member> IReadOnlyAcademyContext.Members => Members;
	IQueryable<Professional> IReadOnlyAcademyContext.Professionals => Professionals;
	IQueryable<Exercise> IReadOnlyAcademyContext.Exercises => Exercises;
	IQueryable<Plan> IReadOnlyAcademyContext.Plans => Plans;
	protected override void OnModelCreating(ModelBuilder b)
	{
		base.OnModelCreating(b);
		foreach (var entity in b.Model.GetEntityTypes().Where(e => e.ClrType.IsSubclassOf(typeof(TraininApp.Domain.Common.BaseEntity))))
		{ b.Entity(entity.ClrType).Property("Id").ValueGeneratedNever(); b.Entity(entity.ClrType).Property("CreatedAt").IsRequired(); b.Entity(entity.ClrType).Property("Active").HasDefaultValue(true); }
		foreach (var property in b.Model.GetEntityTypes().SelectMany(e => e.GetProperties()).Where(p => p.ClrType == typeof(decimal))) property.SetColumnType("decimal(18,2)");
		b.Entity<Member>().HasIndex(x => x.TaxId).IsUnique(); b.Entity<Professional>().HasIndex(x => x.TaxId).IsUnique(); b.Entity<Professional>().HasIndex(x => x.LicenseNumber).IsUnique();
		b.Entity<Enrollment>().Property(x => x.RowVersion).IsRowVersion(); b.Entity<Installment>().Property(x => x.RowVersion).IsRowVersion();
		b.Entity<Enrollment>().HasMany(x => x.Installments).WithOne().HasForeignKey(x => x.EnrollmentId).OnDelete(DeleteBehavior.Cascade);
		b.Entity<WorkoutPlan>().HasMany(x => x.Items).WithOne().HasForeignKey(x => x.WorkoutPlanId).OnDelete(DeleteBehavior.Cascade);
	}
}

