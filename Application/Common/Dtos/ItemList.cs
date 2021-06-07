using System.Collections.Generic;

namespace Application.Common.Dtos
{
    public class ItemList<TItem>
    {
        public int TotalCount { get; init; }
        public IEnumerable<TItem> Items { get; init; }
    }
}
