using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repository.LanguageRepository;
using Infrastructure.Configurations.Queries;
using Infrastructure.Services;

namespace Application.Queries.LanguageQueries.GetAllLanguage
{
    public class GetAllLanguageQueryHandler : IQueryHandler<GetAllLanguageQuery, GetAllLanguageResponse>
    {
        private readonly ILanguageRepository _repository;
        private readonly AuthService _authService;

        public GetAllLanguageQueryHandler(ILanguageRepository repository, AuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }
        public async Task<GetAllLanguageResponse> Handle(GetAllLanguageQuery query, CancellationToken cancellationToken)
        {
            var data = _repository.GetAsDictionary(query.Request.LanguageCode);

            return new GetAllLanguageResponse()
            {
                Response = data
            };
        }
    }
}
