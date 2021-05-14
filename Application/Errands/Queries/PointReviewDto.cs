using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Errands.Queries
{
    public class PointReviewDto : IMapFrom<PointReview>
    {
        public Guid PerimeterId { get; init; }
        public string PerimeterName { get; init; }
        public Guid PointId { get; init; }
        public int Order { get; init; }
        public Guid TrapId { get; init; }
        public string TrapName { get; init; }

        public IReadOnlyCollection<PointReviewRecordDto> Records { get; init; } = new List<PointReviewRecordDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PointReview, PointReviewDto>()
              .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Point.Order))
              .ForMember(dest => dest.PerimeterId, opt => opt.MapFrom(src => src.Point.PerimeterId))
              .ForMember(dest => dest.PerimeterName, opt => opt.MapFrom(src => src.Point.Perimeter.PerimeterName))
              .ForMember(dest => dest.PointId, opt => opt.MapFrom(src => src.Point.Id))
              .ForMember(dest => dest.TrapId, opt => opt.MapFrom(src => src.Point.Trap.Id))
              .ForMember(dest => dest.TrapName, opt => opt.MapFrom(src => src.Point.Trap.TrapName));
        }
    }
}
