// GB.SQLChatBot.Business/Queries/GetPersonAnnualLeaveActionsQuery.cs
using System.Collections.Generic;
using GB.SQLChatBot.Data.DTOs;

namespace GB.SQLChatBot.Business.Leave.Queries
{
    // Belirli bir kişiye ait izin hareketlerini getirme
    public class GetPersonAnnualLeaveActionsByPersonRefQuery : MediatR.IRequest<List<PersonAnnualLeaveActionDTO>>
    {
        public int PersonRef { get; set; }
    }

    // Son N gün içindeki tüm izin hareketlerini getirme
    public class GetRecentAnnualLeaveActionsQuery : MediatR.IRequest<List<PersonAnnualLeaveActionDTO>>
    {
        public int Days { get; set; }
    }

    // Belirli bir izin tipi için hareketleri getirme
    public class GetAnnualLeaveActionsByActionTypeQuery : MediatR.IRequest<List<PersonAnnualLeaveActionDTO>>
    {
        public int ActionTypeRef { get; set; }
    }

    // Tüm izin hareketlerini getirme (genel sorgu)
    public class GetAllAnnualLeaveActionsQuery : MediatR.IRequest<List<PersonAnnualLeaveActionDTO>>
    {
        // Parametreye ihtiyaç yok
    }
}