using AutoMapper;
using GB.SQLChatBot.Business.Leave.Models;
using GB.SQLChatBot.Data;
using GB.SQLChatBot.Data.EF.Tables;

namespace GB.SQLChatBot.Business.Leave;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<tb_PersonAnnualLeaveAction, PersonAnnualLeaveActionModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PersonAnnualLeaveActionRef))
            .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonRef))
            .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(src => src.ActionTypeRef))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));
    }
}
