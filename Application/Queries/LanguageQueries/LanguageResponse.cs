using System.Collections.Generic;
using Core.Models;

namespace Application.Queries.LanguageQueries
{
    public class LanguageResponse
    {
        public string Language { get; set; }
        public List<KeyPocoModel> Keys { get; set; }
    }
}
