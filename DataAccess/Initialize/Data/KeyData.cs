using System.Collections.Generic;
using Core.Enums;
using Domain.Entities.Lang;

namespace DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {
        public static List<Key> BuildKeyList()
        {
            var az = LanguageStringEnum.az_AZ;
            var en = LanguageStringEnum.en_US;

            var list = new List<Key>()
            {
                new Key()
                {
                    Label = "auth",
                    Children = new List<Key>()
                    {
                        new Key()
                        {
                            Label = "username",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Istifadəçi adı"
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Username"
                                }
                            }
                        },
                        new Key()
                        {
                            Label = "password",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Şifrə"
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Password"
                                }
                            }
                        },
                        new Key()
                        {
                            Label = "loginBtn",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Daxil Ol"
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Log in"
                                }
                            }
                        },
                        new Key()
                        {
                            Label = "logoutBtn",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Çıxış"
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Log out"
                                }
                            }
                        },

                    }
                },
                new Key()
                {
                    Label = "validationMessages",
                    Children = new List<Key>()
                    {
                        new Key()
                        {
                            Label ="invalidPassword",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "Istifadəçi adı və ya şifrə səhvdir."
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "Username or password is not valid."
                                }
                            }
                        },
                        new Key()
                        {
                            Label ="unauditedFigures",
                            Languages = new List<LanguageKey>()
                            {
                                new LanguageKey()
                                {
                                    LanguageId = az,
                                    Value = "{{value}} məlumatları audit olunmamışdır."
                                },
                                new LanguageKey()
                                {
                                    LanguageId = en,
                                    Value = "{{value}} figures are unaudited."
                                }
                            }
                        }
                    }
                },
            };


            return list;
        }
    }
}
