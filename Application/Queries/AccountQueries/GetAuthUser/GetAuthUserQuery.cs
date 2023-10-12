using Infrastructure.Configurations.Queries;

namespace Application.Queries.AccountQueries.GetAuthUser
{
    public class GetAuthUserQuery : IQuery<GetAuthUserResponse>

    {
        public GetAuthUserQuery(GetAuthUserRequest request)
        {
            Request = request;
        }

        public GetAuthUserRequest Request { get; set; }
    }
}
