using AutoMapper;
using ezyGo.Admin.Domain.Models;
using ezyGo.Admin.Storage.Entities;

namespace ezyGo.Admin.Domain.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BusCompany, BusCompanyEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<BusCompanyEntity, BusCompany>();

        CreateMap<Bus, BusEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(d => d.BusCompanyEntityId,
               opt => opt.MapFrom(src => src.BusCompanyId));
           // .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));

        CreateMap<BusEntity, Bus>()
            .ForMember(d => d.BusCompanyId,
               opt => opt.MapFrom(src => src.BusCompanyEntityId))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName));

        CreateMap<Station, TrainStation>()
           .ForMember(d => d.TrainStationName,
               opt => opt.MapFrom(src => src.StationName))
           .ForMember(dest => dest.TrainStationDescription,
               opt => opt.MapFrom(src => src.StationDescription));

        CreateMap<Station, BusStationEntity>()
           .ForMember(d => d.StationName,
               opt => opt.MapFrom(src => src.StationName))
           .ForMember(dest => dest.StationDescription,
               opt => opt.MapFrom(src => src.StationDescription));

        CreateMap<BusStationEntity, Station>();

        CreateMap<ezyGo.Admin.Domain.Models.Geo, ezyGo.Admin.Storage.Entities.Geo>();
        CreateMap<ezyGo.Admin.Storage.Entities.Geo, ezyGo.Admin.Domain.Models.Geo>();

        //Map Route
        CreateMap<Route, RouteEntity>().ReverseMap();

    }
}
