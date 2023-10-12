using Domain.Common.Configurations;

namespace Domain.Entities.App
{
    public class AppDomain : Entity
    {
        public int Id { get; set; }
        public string Domain { get; set; }
    }
}
