using DeleteDefect.Models;
using Microsoft.EntityFrameworkCore;

namespace DeleteDefect.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<DefectModel> Defect_Results { get; set; }
    public DbSet<LocationModel> Locations { get; set; }
    public DbSet<DefectNameModel> Defect_Names { get; set; }
    public DbSet<InspectorModel> AspNetUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfigurasi join berdasarkan kolom `InspectorNIK` dan `NIK`
        modelBuilder.Entity<DefectModel>()
            .HasOne(d => d.Inspector)
            .WithMany() 
            .HasForeignKey(d => d.InspectorId)
            .HasPrincipalKey(i => i.NIK);
    }

}