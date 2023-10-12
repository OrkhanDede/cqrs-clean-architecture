using Application.Queries.PermissionQueries;
using AutoMapper;
using Domain.Entities.Identity;

namespace Application.Mapping
{
    public class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {


            #region Permission

            CreateMap<Permission, PermissionResponse>();
            #endregion   
            #region Permission Category

            CreateMap<PermissionCategory, PermissionCategoryResponse>();
            CreateMap<PermissionCategoryPermission, PermissionCategoryRelationResponse>()
                .ForMember(c => c.RelationId, opt => opt.MapFrom(m => m.Id))
                .ForMember(c => c.Category, opt => opt.MapFrom(m => m.Category))
                .ForMember(c => c.Permission, opt => opt.MapFrom(m => m.Permission));
            #endregion




        }
    }
}
