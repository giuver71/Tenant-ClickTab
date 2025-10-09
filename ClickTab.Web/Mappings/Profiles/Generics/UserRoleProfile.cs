using AutoMapper;
using ClickTab.Core.DAL.Models.Generics;
using ClickTab.Web.Mappings.ModelsDTO.Generics;
using System;

namespace ClickTab.Web.Mappings.Profiles.Generics
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRole, UserRoleDTO>().ConvertUsing((model, dto, ctx) =>
            {
                UserRoleDTO userRoleDTO = new UserRoleDTO()
                {
                    ID = model.ID,
                    FK_User = model.FK_User,
                    FK_Role = model.FK_Role,
                    Role = ctx.Mapper.Map<RoleDTO>(model.Role),
                    FK_InsertUser = model.FK_InsertUser,
                    InsertDate = model.InsertDate,
                    FK_UpdateUser = model.FK_UpdateUser,
                    UpdateDate = model.UpdateDate,
                };

                return userRoleDTO;
            });


            CreateMap<UserRoleDTO, UserRole>().ConvertUsing((dto, model, ctx) =>
            {
                UserRole userRole = new UserRole()
                {
                    ID = dto.ID,
                    FK_Role = dto.Role == null ? dto.FK_Role : dto.Role.ID,
                    FK_InsertUser = dto.FK_InsertUser,
                    InsertDate = dto.InsertDate,
                    FK_UpdateUser = dto.FK_UpdateUser,
                    UpdateDate = dto.UpdateDate,
                };

                return userRole;
            });
        }
    }
}
