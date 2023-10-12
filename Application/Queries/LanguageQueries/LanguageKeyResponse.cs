using System.Collections.Generic;

namespace Application.Queries.LanguageQueries
{
    public class LanguageKeyResponse
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public List<LanguageKeyResponse> Children { get; set; } = new List<LanguageKeyResponse>();
        public string Value { get; set; }
    }
}
