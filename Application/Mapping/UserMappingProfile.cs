using System;
using System.Linq;
using Application.Commands.UserCommands.CreateUser;
using Application.Commands.UserCommands.UpdateUser;
using Application.Queries.UserQueries;
using AutoMapper;
using Domain.Entities.Identity;

namespace Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            #region User
            CreateMap<User, UserResponse>();
            CreateMap<User, UserInfoResponse>();

            CreateMap<CreateUserRequest, User>()
                .ForMember(c => c.Roles, opt => opt.Ignore())
                .ForMember(c => c.DirectivePermissions, opt => opt.Ignore());
            CreateMap<UpdateUserRequest, User>()
                .ForMember(c => c.Roles, opt => opt.Ignore())
                .ForMember(c => c.DirectivePermissions, opt => opt.Ignore());
            #endregion
        }
    }
}
