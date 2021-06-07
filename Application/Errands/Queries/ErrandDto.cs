using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
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
        public int DaysOverdue { get; init; }
        public bool OnDemand { get; init; }

        public ICollection<PointReviewDto> Points { get; init; } = new List<PointReviewDto>();

        private static DateTime GetDueDate(DateTime dueDate, CurrentUser currentUser, DateTime currentDatetime)
        {
            if (currentUser.Role == UserRole.Provider)
                return dueDate;

            return dueDate.Date < currentDatetime.Date ? currentDatetime.Date : dueDate.Date;
        }

        public static ErrandDto Map(Errand errand, CurrentUser currentUser, DateTime currentDatetime)
        {
            var perimeters = errand.Facility.Perimeters.ToDictionary(p => p.Id);

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
                OnDemand = errand.OnDemand,
                DueDate = GetDueDate(errand.DueDate, currentUser, currentDatetime).ToString("d"),
                DaysOverdue = currentUser.Role == UserRole.Employee ? 0 : (currentDatetime.Date - errand.DueDate.Date).Days,
                Points = errand.Points.Select(p => new PointReviewDto
                {
                    PointId = p.PointId,
                    PerimeterId = p.Point.PerimeterId,
                    PerimeterName = perimeters[p.Point.PerimeterId].PerimeterName,
                    Order = p.Point.Order,
                    TrapId = p.Point.TrapId,
                    TrapName = p.Point.Trap.TrapName,
                    TrapColor = p.Point.Trap.Color,
                    Records = p.Point.Trap.Fields.Select(r => new PointReviewRecordDto
                    {
                        FieldId = r.Id,
                        FieldName = r.FieldName,
                        FieldType = r.FieldType,
                        OptionList = r.OptionList.ToArray()
                    }).ToList()
                }).OrderBy(o => o.Order).ToList()
            };
        }
    }
}
