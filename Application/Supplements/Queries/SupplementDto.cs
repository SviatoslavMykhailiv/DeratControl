using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Supplements.Queries {
  public class SupplementDto : IMapFrom<Supplement> {
    public Guid SupplementId { get; init; }
    public string SupplementName { get; init; }
    public DateTime ExpirationDate { get; init; }
    public string CertificatePath { get; init; }
    public void Mapping(Profile profile) {
      profile.CreateMap<Supplement, SupplementDto>().ForMember(dest => dest.SupplementId, opt => opt.MapFrom(src => src.Id));
    }
  }
}
