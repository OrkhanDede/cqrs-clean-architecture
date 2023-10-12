using System;
using Core.Enums;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LocalizeAttribute:Attribute
    {
        public string Key { get; private set; }
        public LanguageEnum LanguageEnum { get; private set; }
        public LocalizeAttribute(string key, LanguageEnum languageEnum)
        {
            this.Key = key;
            this.LanguageEnum = languageEnum;
        }
    }
}
