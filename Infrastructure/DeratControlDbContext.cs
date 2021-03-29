using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure {
  public class DeratControlDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDeratControlDbContext {
    public static readonly LoggerFactory _myLoggerFactory =
    new LoggerFactory(new[] {
        new DebugLoggerProvider()
    });

    private readonly ICurrentUserService currentUserService;
    private readonly ICurrentDateService currentDateService;

    public DeratControlDbContext(
      DbContextOptions<DeratControlDbContext> options,
      ICurrentUserService currentUserService,
      ICurrentDateService currentDateService) : base(options) {
      this.currentUserService = currentUserService;
      this.currentDateService = currentDateService;
    }

    public DbSet<Facility> Facilities { get; set; }

    public DbSet<Perimeter> Perimeters { get; set; }

    public DbSet<Errand> Errands { get; set; }

    public DbSet<Trap> Traps { get; set; }

    public DbSet<Supplement> Supplements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeratControlDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseLoggerFactory(_myLoggerFactory);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
      foreach (var entry in ChangeTracker.Entries<AuditableEntity>()) {
        switch (entry.State) {
          case EntityState.Added:
          case EntityState.Modified:
            entry.Entity.ModifiedBy = currentUserService.UserId;
            entry.Entity.ModifiedAt = currentDateService.CurrentDate;
            break;
        }
      }

      return base.SaveChangesAsync(cancellationToken);
    }
  }
}
