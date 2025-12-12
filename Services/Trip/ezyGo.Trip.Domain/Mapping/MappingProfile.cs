using AutoMapper;
using ezyGo.Trip.Domain.Models;
using ezyGo.Trip.Storage.Entities;

namespace ezyGo.Trip.Domain.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.ServiceClients.AdminClient.Models.TripTemplateClientResponse,
                 Models.TemplateTripResponse>()
          .ForMember(dest => dest.TripTemplateId,
              opt => opt.MapFrom(src => src.TemplateId))
          .ForMember(dest => dest.Description,
              opt => opt.MapFrom(src => src.TripDescription))
          .ForMember(dest => dest.BusName,
              opt => opt.MapFrom(src => src.BusName))
          .ForMember(dest => dest.TotalCapacity,
              opt => opt.MapFrom(src => src.TotalCapacity))
          // BusType not available → leave empty
          .ForMember(dest => dest.BusType,
              opt => opt.MapFrom(_ => string.Empty))
          .ForMember(dest => dest.BusCompanyId,
              opt => opt.MapFrom(src => src.CompanyId))
          .ForMember(dest => dest.CompanyName,
              opt => opt.MapFrom(src => src.CompanyName))
          .ForMember(dest => dest.RouteId,
              opt => opt.MapFrom(src => src.RouteId ?? 0))
          .ForMember(dest => dest.StartingPoint,
              opt => opt.MapFrom(src => src.StartingPoint))
          .ForMember(dest => dest.EndingPoint,
              opt => opt.MapFrom(src => src.EndingPoint))
          .ForMember(dest => dest.BaseFare,
              opt => opt.MapFrom(src => src.BaseFare))
          .ForMember(dest => dest.DepartureTime,
              opt => opt.MapFrom(src => src.DepartureTime))
          .ForMember(dest => dest.ArrivalTime,
              opt => opt.MapFrom(src => src.ArrivalTime))
          .ForMember(dest => dest.Stoppages,
              opt => opt.MapFrom(src => src.Stoppages ?? string.Empty))
          .ForMember(dest => dest.StoppageList,
              opt => opt.MapFrom(src => src.StoppagesList));



        CreateMap<TripDetailsModel, TripDetails>().ReverseMap();


        CreateMap<TemplateTripResponse, TripDetailsModel>()
            .ForMember(dest => dest.CompanyId,
               opt => opt.MapFrom(src => src.BusCompanyId))
            .ForMember(dest => dest.StoppageList, opt => opt.Ignore())
            .ForMember(dest => dest.TravelDate, opt => opt.Ignore());


        CreateMap<TripRequest, UserTripRequest>().ReverseMap();

        CreateMap<Seat, SeatModel>().ReverseMap();
    }
}
