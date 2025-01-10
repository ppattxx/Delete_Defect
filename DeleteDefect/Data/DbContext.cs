using DeleteDefect.Models;
using Microsoft.EntityFrameworkCore;

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
            .HasOne(d => d.Inspector) // Properti navigasi ke Inspector
            .WithMany() // Tidak ada properti navigasi balik di Inspector
            .HasForeignKey(d => d.InspectorId) // Kolom foreign key di Defect_Results
            .HasPrincipalKey(i => i.NIK); // Kolom primary key di Inspectors
    }

}
