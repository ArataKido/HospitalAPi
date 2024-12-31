

using Accounts.DTO;
using Accounts.Entity;
using AutoMapper;

namespace Accounts.Utils.Mappers;
public class AccountMappingProfile : Profile
{

    public AccountMappingProfile()
    {
        CreateMap<AccountCreateDTO, Account>()
            .ForMember(dest => dest.FullName, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.UpdateFullName());

        CreateMap<AccountUpdateDTO, Account>()
            .ForMember(dest => dest.FullName, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.UpdateFullName())
            .ForAllMembers(opts =>
            {
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

        CreateMap<AccountPrivateUpdateDTO, Account>()
            .ForMember(dest => dest.FullName, opt => opt.Ignore())
            .AfterMap((src, dest) => dest.UpdateFullName())
            .ForAllMembers(opts =>
            {
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

        CreateMap<Account, AccountResponseDTO>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.AccountRoles.Select(ar => ar.Role.Name)));
    }

}