using GB.SQLChatBot.Business.Leave.Queries;
using GB.SQLChatBot.Data;
using GB.SQLChatBot.Data.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GB.SQLChatBot.Data.EF;
using System.Linq;

namespace GB.SQLChatBot.Business.Handlers.Leave
{
    public class GetAllLeaveActionsHandler : IRequestHandler<GetAllAnnualLeaveActionsQuery, List<PersonAnnualLeaveActionDTO>>
    {
        private readonly LeaveContext _context;

        public GetAllLeaveActionsHandler(LeaveContext context)
        {
            _context = context;
        }

        public async Task<List<PersonAnnualLeaveActionDTO>> Handle(GetAllAnnualLeaveActionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.tb_PersonAnnualLeaveAction
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.CreatedDate)
                .Select(a => new PersonAnnualLeaveActionDTO
                {
                    PersonAnnualLeaveActionRef = a.PersonAnnualLeaveActionRef,
                    PersonRef = a.PersonRef,
                    ActionTypeRef = a.ActionTypeRef,
                    Duration = a.Duration,
                    CreatedDate = a.CreatedDate
                })
                .ToListAsync(cancellationToken);
        }
    }
}
