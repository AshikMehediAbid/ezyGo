using AutoMapper;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;

namespace ezyGo.Admin.Storage.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Station, TrainStation>()
            .ForMember(d => d.TrainStationName,
                opt => opt.MapFrom(src => src.StationName))
            .ForMember(dest => dest.TrainStationDescription,
                opt => opt.MapFrom(src => src.StationDescription))
            .ForMember(dest => dest.TrainStationId, opt => opt.Ignore());

        CreateMap<ezyGo.Admin.Domain.Models.Geo, ezyGo.Admin.Storage.Entities.Geo>();
        CreateMap<ezyGo.Admin.Storage.Entities.Geo, ezyGo.Admin.Domain.Models.Geo>();
    }

}
