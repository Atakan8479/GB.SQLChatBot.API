// GB.SQLChatBot.Data/Repositories/IPersonAnnualLeaveActionRepository.cs
using GB.SQLChatBot.Data.EF.Tables;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GB.SQLChatBot.Data.Repositories
{
    public interface IPersonAnnualLeaveActionRepository
    {
        Task<IEnumerable<tb_PersonAnnualLeaveAction>> GetAllAsync();
        Task<IEnumerable<tb_PersonAnnualLeaveAction>> FindAsync(Expression<Func<tb_PersonAnnualLeaveAction, bool>> predicate);
        Task<tb_PersonAnnualLeaveAction?> GetByIdAsync(int id);
        // Sadece okuma amaçlı olduğu için Add/Update/Delete metodları eklenmedi.
    }
}
