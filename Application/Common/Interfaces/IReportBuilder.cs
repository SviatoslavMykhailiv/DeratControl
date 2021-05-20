using System.Collections;
using System.Collections.Generic;

namespace Application.Common.Interfaces {
  public enum Align {
    Left,
    Center,
    Right
  }

  public class Table : IEnumerable<Row> {
    private readonly List<Row> rows = new List<Row>();

    public Table(IEnumerable<string> columns) {
      Columns = columns;
    }

    public IEnumerable<string> Columns { get; }

    public IEnumerator<Row> GetEnumerator() {
      return rows.GetEnumerator();
    }

    public Row NewRow() {
      var row = new Row(this);
      rows.Add(row);

      return row;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }

  public class Row {
    public Row(Table table) {
      this.table = table;

      row = new Dictionary<string, string>();

      foreach (var column in table.Columns) {
        row.Add(column, string.Empty);
      }
    }

    private readonly IDictionary<string, string> row;
    private readonly Table table;

    public string this[string column] {
      get {
        return row[column];
      }
      set {
        row[column] = value;
      }
    }
  }

  public interface IReportBuilder {
    IReportBuilder AddTitle(string title);
    IReportBuilder AddText(string content, Align align, uint fontSize);
    IReportBuilder AddVerticalSpace();
    IReportBuilder AddCheckbox(bool isChecked, string caption);
    IReportBuilder AddTable(Table table);
    IReportBuilder AddSignature(byte[] signature, Align align);
    byte[] GetReport();
  }
}
