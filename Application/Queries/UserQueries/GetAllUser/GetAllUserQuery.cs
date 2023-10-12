using Infrastructure.Configurations.Queries;

namespace Application.Queries.UserQueries.GetAllUser
{
    public class GetAllUserQuery : IQuery<GetAllUserResponse>
    {
        public GetAllUserQuery(GetAllUserRequest request)
        {
            Request = request;
        }

        public GetAllUserRequest Request { get; set; }

    }
}
