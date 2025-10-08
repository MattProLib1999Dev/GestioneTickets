using AutoMapper;
using GestioneTickets.DTOs;
using GestioneTickets.Model;
public class MapperConfig : Profile
{
    public MapperConfig()
    {
        // da DTO a Entity
        CreateMap<GetAllAccountDto, Account>();

        // opzionale: da Entity a DTO
        CreateMap<Account, GetAllAccountDto>();
    }

    public static IMapper InitializeAutomapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperConfig>();
        });

        return config.CreateMapper();
    }
}
