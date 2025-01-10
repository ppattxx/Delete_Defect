using DeleteDefect.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<DefectModel> Defect_Results { get; set; }
    public DbSet<LocationModel> Locations { get; set; }
}
