using System.Collections.Generic;

namespace Core.Models
{
    public class KeyPocoModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public List<KeyPocoModel> Children { get; set; } = new List<KeyPocoModel>();
        public string Value { get; set; }
    }
}
