using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DeratControlDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDeratControlDbContext
    {
        public static readonly LoggerFactory _myLoggerFactory =
        new LoggerFactory(new[] {
        new DebugLoggerProvider()
        });

        private readonly ICurrentDateService currentDateService;

        public DeratControlDbContext(
          DbContextOptions<DeratControlDbContext> options,
          ICurrentDateService currentDateService,
          ICurrentUserProvider currentUserProvider) : base(options)
        {
            this.currentDateService = currentDateService;
            User = currentUserProvider?.User;
        }

        public DbSet<Facility> Facilities { get; set; }

        public DbSet<Perimeter> Perimeters { get; set; }

        public DbSet<Errand> Errands { get; set; }

        public DbSet<Trap> Traps { get; set; }

        public DbSet<Supplement> Supplements { get; set; }

        public CurrentUser User { get; }

        public DbSet<CompletedErrand> CompletedErrands { get; set; }

        public DbSet<CompletedPointReview> CompletedPointReviews { get; set; }

        public DbSet<Point> Points { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeratControlDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_myLoggerFactory);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = User.UserId;
                        entry.Entity.ModifiedAt = currentDateService.CurrentDate;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
