using Microsoft.EntityFrameworkCore;
using WebAppSummerSchool;
using WebAppSummerSchool.Models;
public class ApplicationDbContext : DbContext
{
    public DbSet<TaskObject> TaskObject { get; set; }
    public DbSet<UserObject> UserObject { get; set; }
    public ApplicationDbContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserObject>()
            .HasMany<TaskObject>()
            .WithOne(tas => tas.UserObject)
            .HasForeignKey(t => t.UserId);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SumrSchBarsTaskManager;Username=postgres;Password=12345asd;");
    }

}