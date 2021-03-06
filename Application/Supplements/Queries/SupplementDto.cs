using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Supplements.Queries
{
    public class SupplementDto : IMapFrom<Supplement>
    {
        public Guid SupplementId { get; init; }
        public string SupplementName { get; init; }
        public DateTime ExpirationDate { get; init; }
        public string Certificate { get; init; }
        public bool IsOverdue { get; private set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Supplement, SupplementDto>()
              .ForMember(dest => dest.SupplementId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.CertificateFilePath));
        }

        public void SetIsOverdue(DateTime currentDate)
        {
            IsOverdue = ExpirationDate.Date < currentDate.Date;
        }
    }
}
