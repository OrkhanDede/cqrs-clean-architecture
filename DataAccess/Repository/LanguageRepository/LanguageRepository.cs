using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Models;
using Data;
using DataAccess.Initialize.Data;
using Domain.Entities.Lang;

namespace DataAccess.Repository.LanguageRepository
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository, IRepositoryIdentifier
    {
        private readonly IKeyRepository _keyRepository;
        private readonly ApplicationDbContext _context;

        public LanguageRepository(IKeyRepository keyRepository, ApplicationDbContext context) : base(context)
        {
            _keyRepository = keyRepository;
            _context = context;
        }

        private List<KeyPocoModel> GetSubKeys(int parentId, string languageCode)
        {
            var keys = _keyRepository.FindBy(c => c.ParentId == parentId, c => c.Languages).Select(c => new KeyPocoModel()
            {
                Id = c.Id,
                Label = c.Label,
                Value = c.Languages.Any(e => e.LanguageId == languageCode) ? c.Languages.FirstOrDefault(m => m.LanguageId == languageCode).Value : null

            }).ToList();
            foreach (var key in keys)
            {
                key.Children = GetSubKeys(key.Id, languageCode);
            }
            return keys;
        }

        public List<KeyPocoModel> GetKeys(string languageCode)
        {
            var data = _keyRepository.FindBy(c => c.ParentId == null).Select(c => new KeyPocoModel()
            {
                Id = c.Id,
                Label = c.Label,
            }).ToList();
            foreach (var child in data)
                child.Children = GetSubKeys(child.Id, languageCode);

            return data;
        }


        private Dictionary<string, object> ParseToObject(List<KeyPocoModel> data)
        {

            var dic = new Dictionary<string, object>();
            foreach (var item in data)
            {
                if (item.Children.Any())
                {
                    var dics = ParseToObject(item.Children);
                    dic.Add(item.Label, dics);
                }
                else
                {
                    dic.Add(item.Label, item.Value);
                }

            }

            return dic;
        }
        public Dictionary<string, object> GetAsDictionary(string languageCode)
        {
            var languages = GetLanguagesAsync(languageCode);
            var dics = new Dictionary<string, object>();
            foreach (var language in languages)
            {
                var keys = ParseToObject(language.Keys);
                dics.Add(language.Language, keys);
            }

            return dics;
        }

        public List<LanguagePocoModel> GetLanguagesAsync(string languageCode)
        {
            Expression<Func<Language, bool>> filterPredicate = null;

            if (!string.IsNullOrEmpty(languageCode))
            {
                var isLanguageCodeExist = !string.IsNullOrEmpty(languageCode);
                filterPredicate = c => (!isLanguageCodeExist || c.Code.ToLower().Equals(languageCode.ToLower()));
            }
            else
            {
                filterPredicate = c => true;
            }

            var languages = FindBy(filterPredicate).Select(c => new LanguagePocoModel()
            {
                Language = c.Code,
            }).ToList();
            foreach (var lng in languages)
            {
                lng.Keys = GetKeys(lng.Language);
            }
            return languages;
        }

        public async Task SetValueAsync(string languageId, int keyId, string value)
        {

            var language = await GetFirstAsync(c =>
                c.Code == languageId, "LanguageKeys").ConfigureAwait(false);
            var languageKey = language.LanguageKeys.FirstOrDefault(c => c.KeyId == keyId);
            if (languageKey != null)
                languageKey.Value = value;

        }

        public async Task ResetAsync()
        {
            _context.LanguageKeys.RemoveRange(_context.LanguageKeys.ToList());
            _context.Languages.RemoveRange(_context.Languages.ToList());
            _context.Keys.RemoveRange(_context.Keys.ToList());
            await _context.SaveChangesAsync().ConfigureAwait(false);

            await _context.Languages.AddRangeAsync(InitializeData.BuildLanguageList());
            await _context.SaveChangesAsync().ConfigureAwait(false);

            await _context.Keys.AddRangeAsync(InitializeData.BuildKeyList());
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
