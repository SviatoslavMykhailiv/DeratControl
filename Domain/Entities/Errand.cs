using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities {
  public class Errand : AuditableEntity {
    public Errand() {
      Reviews = new HashSet<PointReview>();
    }

    public Guid ErrandId { get; set; }
    public Guid FacilityId { get; set; }
    public Guid EmployeeId { get; set; }

    public DateTime OriginalDueDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public ErrandStatus Status { get; set; }
    public string Description { get; set; }
    public string Report { get; set; }

    public Facility Facility { get; set; }
    public IUser Employee { get; set; }
    public ICollection<PointReview> Reviews { get; private set; }

    public void SetPointReview(IReadOnlyCollection<Guid> pointCollection, IDictionary<Guid, Supplement> supplements) {
      var pointReviewDic = Reviews.ToDictionary(r => r.PointId);
      var points = Facility.Perimeters.SelectMany(p => p.Points).ToDictionary(p => p.PointId);

      foreach(var review in Reviews.ToList()) {
        if (pointCollection.Contains(review.PointId)) 
          continue;

        Reviews.Remove(review);
      }

      foreach(var point in pointCollection) {
        if (pointReviewDic.ContainsKey(point)) 
          continue;

        var pointReview = new PointReview {
          Errand = this,
          Point = points[point],
          Status = PointReviewStatus.NotReviewed
        };

        foreach(var field in supplements[points[point].SupplementId].Fields) {
          pointReview.Records.Add(new PointReviewRecord { 
            PointReview = pointReview,
            SupplementField = field,
            Value = string.Empty
          });
        }

        Reviews.Add(pointReview);
      }
    }
  }
}
