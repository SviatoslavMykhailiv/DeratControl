using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces {
  public interface IDeratControlDbContext {
    DbSet<Facility> Facilities { get; }
    DbSet<Perimeter> Perimeters { get; }
    DbSet<Errand> Errands { get; }
    DbSet<Trap> Traps { get; }
    DbSet<Supplement> Supplements { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  }
}
