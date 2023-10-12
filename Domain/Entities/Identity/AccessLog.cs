using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    public class AccessLog
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public string IpAddress { get; set; }
        public string Host { get; set; }
        public DateTime Date { get; set; }

    }
}
