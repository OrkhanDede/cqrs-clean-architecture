using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace DataAccess.Repository.LanguageRepository
{
    public interface ILanguageRepository
    {
        //get hierarchy
        List<KeyPocoModel> GetKeys(string languageCode);
        Dictionary<string, object> GetAsDictionary(string languageCode);
        List<LanguagePocoModel> GetLanguagesAsync(string languageCode);

        Task SetValueAsync(string languageId, int keyId, string value);
        Task ResetAsync();

    }

}
