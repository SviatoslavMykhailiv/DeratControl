using Application.Common.Mappings;
using Application.Traps;
using AutoMapper;
using Domain.Entities;

namespace Application.Perimeters
{
    public class PointFieldValueDTO : IMapFrom<PointFieldValue>
    {
        public FieldDto Field { get; init; }
        public string Value { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PointFieldValue, PointFieldValueDTO>();
        }
    }
}
