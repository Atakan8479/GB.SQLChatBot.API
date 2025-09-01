using System;

namespace GB.SQLChatBot.Business.Leave.Models;

public class PersonAnnualLeaveActionModel
{
    public int Id { get; set; } // PersonAnnualLeaveActionRef
    public int PersonId { get; set; } // PersonRef
    public int ActionTypeId { get; set; } // ActionTypeRef
    public decimal Duration { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsDeleted { get; set; }
}