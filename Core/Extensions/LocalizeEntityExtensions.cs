using System;
using System.Collections.Generic;
using System.Linq;
using Core.Attributes;
using Core.Enums;

namespace Core.Extensions
{
    public static class LocalizeEntityExtensions
    {

        public static T Localize<T>(this T l, string lang) where T : class, new()
        {
            return l.LocalizeFromAttribute(lang);
        }
        public static List<T> Localize<T>(this List<T> list, string lang) where T : class, new()
        {
            return list.LocalizeFromAttribute(lang);
        }

        private static List<T> LocalizeFromAttribute<T>(this List<T> list, string lang) where T : class, new()
        {
            list.ForEach(l=>l.LocalizeFromAttribute(lang));
            return list;
        }
        private static T LocalizeFromAttribute<T>(this T model, string lang) where T :  class, new()
        {
            const string az = LanguageStringEnum.az_AZ;
            const string en = LanguageStringEnum.en_US;
            LanguageEnum languageEnum = LanguageEnum.En;
            if (string.IsNullOrEmpty(lang))
            {
                lang = en;
            }

            if (lang.ToLower() == az.ToLower())
            {
                languageEnum = LanguageEnum.Az;
            }

            var properties = model.GetType().GetProperties();
            foreach (var property in properties)
            {
                var localizeEntityAttribute =Attribute.GetCustomAttribute(property, typeof(LocalizeAttribute)) as LocalizeAttribute;
                if (localizeEntityAttribute != null && localizeEntityAttribute.LanguageEnum == languageEnum)
                {
                    var value = property.GetValue(model,null);
                    var keyProperty=properties.First(x => x.Name.ToLower() == localizeEntityAttribute.Key.ToLower());
                    if (value != null)
                    {
                        keyProperty.SetValue(model, value);
                    }
                }
            }
            
            return model;
        }
    }
}
