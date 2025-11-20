using AutoMapper;
using ezyGo.Auth.Domain.Models;

namespace ezyGo.Auth.Domain.Mapping;

public class MappingProfiles : Profile
{
    public MappingProfiles() 
    {
        CreateMap<UserRegister, Storage.Entities.UserEntity>()
            .ForMember(d => d.PasswordHash, o => o.Ignore())
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.IsActive, o => o.Ignore());
    }
}
