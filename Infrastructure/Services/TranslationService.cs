using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Core.Extensions;
using DataAccess.Repository.LanguageRepository;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class TranslationService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _acceptLanguage;
        private readonly Dictionary<string, object> _languageDic;
        public TranslationService(ILanguageRepository languageRepository, IHttpContextAccessor httpContextAccessor)
        {
            _languageRepository = languageRepository;
            _httpContextAccessor = httpContextAccessor;
            _acceptLanguage = httpContextAccessor.HttpContext.GetAcceptLanguage();

            this._languageDic = GetCurrentLanguageKeys();
        }

        public Dictionary<string, object> GetCurrentLanguageKeys()
        {
            var languageCode = "";


            const string az = LanguageStringEnum.az_AZ;
            const string en = LanguageStringEnum.en_US;

            switch (_acceptLanguage)
            {
                case az:
                    languageCode = az;
                    break;
                case en:
                    languageCode = en;
                    break;
                default:
                    languageCode = en;
                    break;

            }


            var keys = _languageRepository.GetAsDictionary(languageCode)[languageCode] as Dictionary<string, object>;

            return keys;
        }


        public string Search(params string[] keys)
        {
            var val = this.SearchKey(this._languageDic, keys);
            if (!string.IsNullOrEmpty(val))
            {
                return val;
            }
            else return $"{{{{{ string.Join(".", keys)}}}}}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"> example value : dictionary.key.subKey </param>
        /// <returns></returns>
        private string? SearchKey(Dictionary<string, object> dic, params string[] keys)
        {
            foreach (var key in keys)
            {
                var isValueExist = dic.Any(c => c.Key == key);
                if (isValueExist)
                {
                    var keyValue = dic[key];
                    var isDic = keyValue.GetType() == typeof(Dictionary<string, object>);
                    var isValue = keyValue is string;

                    if (isDic)
                    {
                        var newDic = keyValue as Dictionary<string, object>;
                        var keyParams = keys.ToList();
                        keyParams.Remove(key);
                        if (keyParams.Any())
                        {
                            return SearchKey(newDic, keyParams.ToArray());

                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return keyValue as string;
                    }
                }
                else
                {
                    return null;

                }


            }

            return null;
        }

    }
}
