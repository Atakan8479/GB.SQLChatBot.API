// GB.SQLChatBot.Business/DTOs/PersonAnnualLeaveActionDto.cs
using System;

namespace GB.SQLChatBot.Data.DTOs
{
    public class PersonAnnualLeaveActionDTO
    {
        public int PersonAnnualLeaveActionRef { get; set; }
        public int PersonRef { get; set; }
        public int ActionTypeRef { get; set; }
        public decimal Duration { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}