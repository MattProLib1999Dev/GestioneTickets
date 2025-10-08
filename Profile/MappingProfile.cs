using AutoMapper;
using GestioneTickets.Model; // per Account
using GestioneTickets.Model; // per CreateAccountDto

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        // da DTO a IdentityUser (Account)
        CreateMap<CreateAccountDto, Account>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignora l'ID durante la mappatura
            .ForMember(dest => dest.Tickets, opt => opt.Ignore()); // Ignora i ticket durante la mappatura

            

        // da Account a DTO di output
        CreateMap<Account, CreaAccountDtoOutput>();
    }
}
