// GB.SQLChatBot.Data/Repositories/PersonAnnualLeaveActionRepository.cs
using GB.SQLChatBot.Data.EF;
using GB.SQLChatBot.Data.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace GB.SQLChatBot.Data.Repositories
{
    public class PersonAnnualLeaveActionRepository : IPersonAnnualLeaveActionRepository
    {
        private readonly LeaveContext _context;

        public PersonAnnualLeaveActionRepository(LeaveContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<tb_PersonAnnualLeaveAction>> GetAllAsync()
        {
            return await _context.tb_PersonAnnualLeaveAction.ToListAsync();
        }

        public async Task<IEnumerable<tb_PersonAnnualLeaveAction>> FindAsync(Expression<Func<tb_PersonAnnualLeaveAction, bool>> predicate)
        {
            return await _context.tb_PersonAnnualLeaveAction.Where(predicate).ToListAsync();
        }

        public async Task<tb_PersonAnnualLeaveAction?> GetByIdAsync(int id)
        {
            return await _context.tb_PersonAnnualLeaveAction.FindAsync(id);
        }
    }
}