using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Application.Errands.Commands.CompleteErrand {
  public class PointReviewDto {
    public Guid PointId { get; init; }
    public PointReviewStatus Status { get; init; }
    public string Report { get; init; }

    public IReadOnlyCollection<PointReviewRecordDto> Records { get; init; } = new List<PointReviewRecordDto>();

    public Dictionary<string, string> GetValueList() {
      var result = new Dictionary<string, string>();

      foreach (var record in Records)
        result.Add(record.FieldName, record.Value);
      
      return result;
    }
  }
}
