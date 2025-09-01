// GB.SQLChatBot.Business/Handlers/GetPersonAnnualLeaveActionsQueryHandler.cs
using MediatR;
using GB.SQLChatBot.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using GB.SQLChatBot.Data.DTOs;
using GB.SQLChatBot.Business.Leave.Queries;

namespace GB.SQLChatBot.Business.Leave.Handlers
{
    public class GetPersonAnnualLeaveActionsByPersonRefQueryHandler : IRequestHandler<GetPersonAnnualLeaveActionsByPersonRefQuery, List<PersonAnnualLeaveActionDTO>>
    {
        private readonly IPersonAnnualLeaveActionRepository _repository;

        public GetPersonAnnualLeaveActionsByPersonRefQueryHandler(IPersonAnnualLeaveActionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PersonAnnualLeaveActionDTO>> Handle(GetPersonAnnualLeaveActionsByPersonRefQuery request, CancellationToken cancellationToken)
        {
            var actions = await _repository.FindAsync(a => a.PersonRef == request.PersonRef && !a.IsDeleted);
            return actions.Select(a => new PersonAnnualLeaveActionDTO
            {
                PersonAnnualLeaveActionRef = a.PersonAnnualLeaveActionRef,
                PersonRef = a.PersonRef,
                ActionTypeRef = a.ActionTypeRef,
                Duration = a.Duration,
                CreatedDate = a.CreatedDate
            }).ToList();
        }
    }

    public class GetRecentAnnualLeaveActionsQueryHandler : IRequestHandler<GetRecentAnnualLeaveActionsQuery, List<PersonAnnualLeaveActionDTO>>
    {
        private readonly IPersonAnnualLeaveActionRepository _repository;

        public GetRecentAnnualLeaveActionsQueryHandler(IPersonAnnualLeaveActionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PersonAnnualLeaveActionDTO>> Handle(GetRecentAnnualLeaveActionsQuery request, CancellationToken cancellationToken)
        {
            var cutoffDate = DateTime.Now.AddDays(-request.Days);
            var actions = await _repository.FindAsync(a => a.CreatedDate >= cutoffDate && !a.IsDeleted);
            return actions.Select(a => new PersonAnnualLeaveActionDTO
            {
                PersonAnnualLeaveActionRef = a.PersonAnnualLeaveActionRef,
                PersonRef = a.PersonRef,
                ActionTypeRef = a.ActionTypeRef,
                Duration = a.Duration,
                CreatedDate = a.CreatedDate
            }).ToList();
        }
    }

    public class GetAnnualLeaveActionsByActionTypeQueryHandler : IRequestHandler<GetAnnualLeaveActionsByActionTypeQuery, List<PersonAnnualLeaveActionDTO>>
    {
        private readonly IPersonAnnualLeaveActionRepository _repository;

        public GetAnnualLeaveActionsByActionTypeQueryHandler(IPersonAnnualLeaveActionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PersonAnnualLeaveActionDTO>> Handle(GetAnnualLeaveActionsByActionTypeQuery request, CancellationToken cancellationToken)
        {
            var actions = await _repository.FindAsync(a => a.ActionTypeRef == request.ActionTypeRef && !a.IsDeleted);
            return actions.Select(a => new PersonAnnualLeaveActionDTO
            {
                PersonAnnualLeaveActionRef = a.PersonAnnualLeaveActionRef,
                PersonRef = a.PersonRef,
                ActionTypeRef = a.ActionTypeRef,
                Duration = a.Duration,
                CreatedDate = a.CreatedDate
            }).ToList();
        }
    }

    public class GetAllAnnualLeaveActionsQueryHandler : IRequestHandler<GetAllAnnualLeaveActionsQuery, List<PersonAnnualLeaveActionDTO>>
    {
        private readonly IPersonAnnualLeaveActionRepository _repository;

        public GetAllAnnualLeaveActionsQueryHandler(IPersonAnnualLeaveActionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PersonAnnualLeaveActionDTO>> Handle(GetAllAnnualLeaveActionsQuery request, CancellationToken cancellationToken)
        {
            var actions = await _repository.FindAsync(a => !a.IsDeleted); // Sadece silinmemiş kayıtları getir
            return actions.Select(a => new PersonAnnualLeaveActionDTO
            {
                PersonAnnualLeaveActionRef = a.PersonAnnualLeaveActionRef,
                PersonRef = a.PersonRef,
                ActionTypeRef = a.ActionTypeRef,
                Duration = a.Duration,
                CreatedDate = a.CreatedDate
            }).ToList();
        }
    }
}