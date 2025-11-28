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
        CreateMap<Route, RouteEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StartingPointId, opt => opt.MapFrom(src => src.StartingPoint.id))
            .ForMember(dest => dest.EndingPointId, opt => opt.MapFrom(src => src.EndingPoint.id))
            .ForMember(dest => dest.StartingPoint, opt => opt.Ignore())
            .ForMember(dest => dest.EndingPoint, opt => opt.Ignore());

        CreateMap<RouteEntity, Route>()
            .ForMember(dest => dest.StartingPoint, opt => opt.MapFrom(src => src.StartingPoint))
            .ForMember(dest => dest.EndingPoint, opt => opt.MapFrom(src => src.EndingPoint));


        // Map Stoppage
        CreateMap<RouteStoppage, RouteStoppageEntity>()
            .ForMember(dest => dest.BusStationEntityId, opt => opt.MapFrom(src => src.StationId))
            .ForMember(dest => dest.RouteEntityId, opt => opt.MapFrom(src => src.RouteId));

        CreateMap<RouteStoppageEntity, RouteStoppage>()
            .ForMember(dest => dest.StationId, opt => opt.MapFrom(src => src.BusStationEntityId))
            .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteEntityId));
    }
}
