using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable warnings

namespace WebAsm.Models;

public class DB : DbContext
{
    public DB(DbContextOptions<DB> options) : base(options) { }

    // DbSet
    public DbSet<User> Users { get; set; }
    public DbSet<Maintenance> Maintenance { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<Response> Response { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<Area> Areas { get; set; }

    public DbSet<RatingImage> RatingImages { get; set; }
    public DbSet<DBImage> DBImages { get; set; } // Updated to DBImage

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<User>("User")
            .HasValue<Admin>("Admin")
            .HasValue<Resident>("Resident")
            .HasValue<Maintenance>("Maintenance");
    }
}

// Entity Classes -------------------------------------------------------------

#nullable disable warnings

public class DBImage
{
    [Key]
    public int DBImageId { get; set; } // Primary key

    [MaxLength(255)]
    public string ImageURL { get; set; } // Path or URL to the image

    // FK to Facility
    public string FacilityId { get; set; }
    public Facility Facility { get; set; } // Navigation property
}
public class User
{
    [Key, MaxLength(10)]
    public string Id { get; set; }

    [MaxLength(255)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string PhotoURL { get; set; }

    [MaxLength(255)]
    public string Email { get; set; }

    [MaxLength(100), Required]
    public string ResidencePlateNumber { get; set; }

    [MaxLength(100)]
    public string Hash { get; set; }

    public string Status { get; set; }

    public string Discriminator { get; set; }

    // Navigation
    public List<Report> Reports { get; set; } = new List<Report>();
}

public class Admin : User { }
public class Resident : User { }
public class Maintenance : User { }


public class Report
{
    [Key, MaxLength(10)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [MaxLength(30)]
    public string Title { get; set; }

    [MaxLength(255)]
    public string Detail { get; set; }

    [MaxLength(255)]
    public string Photo { get; set; }

    public string Status { get; set; }

    public string Priority { get; set; }

    public DateTime PostedDate { get; set; }

    //FK
    public string UserId { get; set; }

    public string FacilityId { get; set; }

    public string? VendorId { get; set; }

    //Navigation
    public User User { get; set; }
    public Facility Facility { get; set; }

    public Vendor Vendor { get; set; }

    public List<Rating> Ratings { get; set; } = [];
}

public class RatingImage
{
    [Key]
    public int Id { get; set; }

    [MaxLength(255)]
    public string ImageURL { get; set; }
    public string RatingId { get; set; }
    public Rating Rating { get; set; }
}

public class Rating
{
    [Key, MaxLength(10)]
    public string Id { get; set; }

    public int StarCount { get; set; }

    [MaxLength(255)]
    public string Text { get; set; }

    public DateTime RatingDate { get; set; }

    public string ReportId { get; set; }

    public string UserId { get; set; }

    //Navigation
    public Report Report { get; set; }
    public ICollection<RatingImage> RatingImages { get; set; } = new List<RatingImage>();
}


public class Vendor
{

    [Key, MaxLength(10)]
    public string Id { get; set; }

    [MaxLength(50)]
    public string OwnerName { get; set; }

    [MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(16)]
    public string Contact { get; set; }

    [MaxLength(100)]
    public string CompanyName { get; set; }

    [MaxLength(50)]
    public string CompanyType { get; set; }

    public string? ReportId { get; set; }
}



public class Response
{
    [Key, MaxLength(10)]
    public string Id { get; set; }

    [MaxLength(255)]
    public string Text { get; set; }

    public DateTime ReplyDate { get; set; }

    //FK

    public String ReportId { get; set; }

    //Navigation

    public Report Report { get; set; }
}

public class Facility
{
    [Key, MaxLength(10)]
    public string Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }

    //FK
    public string AreaId { get; set; }

    //Navigation
    public Area Area { get; set; }

    public ICollection<DBImage> DBImages { get; set; } = new List<DBImage>();

}

public class Area
{
    [Key, MaxLength(10)]
    public string Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    
    //Navigation
    public Facility Facility { get; set; }
}