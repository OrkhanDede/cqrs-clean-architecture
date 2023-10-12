using Infrastructure.Configurations.Queries;

namespace Application.Queries.LanguageQueries.GetAllLanguage
{
    public class GetAllLanguageQuery : IQuery<GetAllLanguageResponse>
    {
        public GetAllLanguageQuery(GetAllLanguageRequest request)
        {
            Request = request;
        }

        public GetAllLanguageRequest Request { get; set; }
    }
}
