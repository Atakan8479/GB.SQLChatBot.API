using Microsoft.EntityFrameworkCore;
using GB.SQLChatBot.Data.EF.Tables;

namespace GB.SQLChatBot.Data.EF;

public partial class LeaveContext : DbContext
{
    public LeaveContext()
    {
    }

    public LeaveContext(DbContextOptions<LeaveContext> options)
        : base(options)
    {
    }

    public virtual DbSet<tb_PersonAnnualLeaveAction> tb_PersonAnnualLeaveAction { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=TESTCOMMONTR\\SQL2016;Initial Catalog=HR_Leave;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;Command Timeout=300");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
