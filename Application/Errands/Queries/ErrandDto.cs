using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Errands.Queries
{
    public class ErrandDto
    {
        public Guid ErrandId { get; init; }
        public string Description { get; init; }
        public string FacilityName { get; init; }
        public Guid FacilityId { get; init; }
        public Guid EmployeeId { get; init; }
        public string EmployeeName { get; init; }
        public string City { get; init; }
        public string Address { get; init; }
        public string DueDate { get; init; }
        public string CompleteDate { get; init; }
        public short Status { get; init; }
        public int DaysOverdue { get; init; }

        public ICollection<PointReviewDto> Points { get; init; } = new List<PointReviewDto>();

        public static ErrandDto Map(Errand errand, CurrentUser currentUser)
        {
            return new ErrandDto
            {
                ErrandId = errand.Id,
                Description = errand.Description,
                FacilityId = errand.FacilityId,
                FacilityName = currentUser.Role == UserRole.Employee ? errand.Facility.Name : errand.Facility.CompanyName,
                EmployeeId = errand.EmployeeId,
                EmployeeName = errand.Employee.GetFullName(),
                City = errand.Facility.City,
                Address = errand.Facility.Address,
                DueDate = errand.DueDate.ToString("d"),
                CompleteDate = errand.CompleteDate == null ? string.Empty : errand.CompleteDate.Value.ToString("d"),
                Status = errand.GetDaysOverdue() > 0 && currentUser.Role == UserRole.Provider ?
              (short)ErrandStatus.Overdue :
              (short)errand.Status,
                DaysOverdue = currentUser.Role == UserRole.Employee ? 0 : errand.GetDaysOverdue(),
                Points = errand.Points.Select(p => new PointReviewDto
                {
                    PointId = p.PointId,
                    PerimeterId = p.Point.PerimeterId,
                    PerimeterName = p.Point.Perimeter.PerimeterName,
                    Order = p.Point.Order,
                    TrapId = p.Point.TrapId,
                    TrapName = p.Point.Trap.TrapName,
                    Records = p.Records.Select(r => new PointReviewRecordDto
                    {
                        RecordId = r.Id,
                        FieldName = r.Field.FieldName,
                        Value = r.Value,
                        FieldType = r.Field.FieldType,
                        OptionList = r.Field.OptionList
                    }).ToList()
                }).OrderBy(o => o.Order).ToList()
            };
        }
    }
}
