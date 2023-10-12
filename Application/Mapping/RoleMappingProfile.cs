using System;
using System.Linq;
using Application.Commands.RoleCommands.CreateRole;
using Application.Commands.RoleCommands.UpdateRole;
using Application.Queries.RoleQueries;
using AutoMapper;
using Domain.Entities.Identity;

namespace Application.Mapping
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {

            #region Role

            CreateMap<Role, RoleResponse>()
                .ForMember(c => c.Permissions,
                    opt => opt.MapFrom(m => m.PermissionCategory.Select(c => c.PermissionCategoryPermission)));

            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();

            #endregion
        }
    }
}
