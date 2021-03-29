using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Supplements.Queries {
  public class SupplementDto : IMapFrom<Supplement> {
    public Guid SupplementId { get; set; }
    public string SupplementName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string CertificatePath { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap(typeof(Supplement), GetType());
    }
  }
}
