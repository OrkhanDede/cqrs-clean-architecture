using System;
using System.Collections.Generic;
using Domain.Entities.Identity;

namespace DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {

        public static List<User> BuildUserList()
        {
            var list = new List<User>()
            {
                new User { Id = Guid.NewGuid().ToString(), UserName = "admin", Email = "admin@gmail.com", DateCreated = DateTime.Now },
            };
            return list;

        }

    }
}
