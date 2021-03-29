using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public abstract class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class {
    public virtual void Configure(EntityTypeBuilder<TEntity> builder) {
      builder.ToTable(typeof(TEntity).Name);
    }
  }
}
