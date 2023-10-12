using System.Collections.Generic;

namespace Core.Models
{
    public class FilteredDataResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
