﻿using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations {
  public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser> {
    public void Configure(EntityTypeBuilder<ApplicationUser> builder) {
      
    }
  }
}
