using AutoMapper;
using GestioneTickets.Model;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mappa Ticket -> CreateTicketDto
        CreateMap<Ticket, CreateTicketDto>()
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria))
            .ForMember(dest => dest.DataCreazione, opt => opt.MapFrom(src => src.DataCreazione))
            .ForMember(dest => dest.DataChiusura, opt => opt.MapFrom(src => src.DataChiusura))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId));

        // Se vuoi, puoi aggiungere anche il mapping inverso
        CreateMap<CreateTicketDto, Ticket>();
    }
}
