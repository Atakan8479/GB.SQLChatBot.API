using GB.SQLChatBot.Business.Leave.Queries;
using GB.SQLChatBot.Data;
using GB.SQLChatBot.Data.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using GB.SQLChatBot.Data.EF;
using System.Linq;

namespace GB.SQLChatBot.Business.Handlers.Leave
{
    public class GetRecentLeaveActionsHandler : IRequestHandler<GetRecentAnnualLeaveActionsQuery, List<PersonAnnualLeaveActionDTO>>
    {
        private readonly LeaveContext _context;

        public GetRecentLeaveActionsHandler(LeaveContext context)
        {
            _context = context;
        }

        public async Task<List<PersonAnnualLeaveActionDTO>> Handle(GetRecentAnnualLeaveActionsQuery request, CancellationToken cancellationToken)
        {
            var threeYearsAgo = DateTime.Now.AddYears(-3);

            return await _context.tb_PersonAnnualLeaveAction
                .Where(a => !a.IsDeleted && a.CreatedDate > threeYearsAgo)
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
