using System.Collections.Generic;
using Core.Enums;
using Domain.Entities.Lang;

namespace DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {

        public static List<Language> BuildLanguageList()
        {
            var list = new List<Language>
            {
                new Language()
                {
                    Code = LanguageStringEnum.az_AZ , Title = "Azerbaijani"

                }, new Language()
                {
                    Code = LanguageStringEnum.en_US , Title = "English"

                }
            };

            return list;
        }
    }
}
