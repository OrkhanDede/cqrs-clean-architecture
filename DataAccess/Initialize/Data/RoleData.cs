using System;
using System.Collections.Generic;
using Domain.Entities.Identity;

namespace DataAccess.Initialize.Data
{
    public static partial class InitializeData
    {

        public static List<Role> BuildRoleList()
        {
            var list = new List<Role>()
            {
                new Role() { Id = Guid.NewGuid().ToString(), Name = "ADMIN" },


            };
            return list;

        }

    }
}
