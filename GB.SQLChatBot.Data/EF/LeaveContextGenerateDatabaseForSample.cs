using System.Threading.Tasks;
using Pars.Core.IInitializer;

namespace GB.SQLChatBot.Data.EF;

public class LeaveContextGenerateDatabaseForSample : IInitializer
{
    private readonly LeaveContext _context;
    public LeaveContextGenerateDatabaseForSample(LeaveContext context)
    {
        _context = context;
    }
    public Task InitializeAsync()
    {
        return _context.Database.EnsureCreatedAsync();
    }
}
