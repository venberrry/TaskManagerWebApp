using Microsoft.EntityFrameworkCore;
using WebAppSummerSchool;

public class ApplicationDbContext : DbContext
{
    public DbSet<TaskObject> TaskObject { get; set; }
    public ApplicationDbContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SumrSchBarsTaskManager;Username=postgres;Password=12345asd;");
    }

}