using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAsm.Models;

#nullable disable warnings

//Jia Hui Part
public class ReportVM
{

    [StringLength(30)]
    public string Title { get; set; }

    [StringLength(255)]
    public string Detail { get; set; }


    public IFormFile Photo { get; set; }

    [DataType(DataType.Date)]
    public DateTime PostedDate { get; set; }

    [Display(Name = "Area")]
    public string AreaId { get; set; }



    [Display(Name = "Facility")]
    public string FacilityId { get; set; }
}

public class ReportEditVM
{

    public string Id { get; set; }

    [StringLength(30)]
    public string Title { get; set; }

    [StringLength(255)]
    public string Detail { get; set; }

    public string? PhotoURL { get; set; }

    public IFormFile? Photo { get; set; }

    [DataType(DataType.Date)]
    public DateTime PostedDate { get; set; }

    [Display(Name = "Area")]
    public string AreaId { get; set; }



    [Display(Name = "Facility")]
    public string FacilityId { get; set; }



}

public class ResponseVM
{

    [MaxLength(255)]
    public string Text { get; set; }

    public string ReportId { get; set; }

}

public class ResponseEditVM
{

    public string Id { get; set; }


    [MaxLength(255)]
    public string Text { get; set; }

    public string ReportId { get; set; }

}

public class ReportStatusVM
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string Detail { get; set; }

    public IFormFile Photo { get; set; }

    [DataType(DataType.Date)]
    public DateTime PostedDate { get; set; }

    [Display(Name = "Area")]
    public string AreaId { get; set; }


    [Display(Name = "Facility")]
    public string FacilityId { get; set; }

    public string Status { get; set; }
    public string Priority { get; set; }
}

public class ReportEditStatusVM
{
    public string Id { get; set; }

    [Required]
    public string Status { get; set; }

    [Required]
    public string Priority { get; set; }
}

//Jian Bin Part
public class LoginVM
{
    [StringLength(100, MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Resident Plate Number")]
    public string ResidencePlateNumber { get; set; }

    public bool RememberMe { get; set; }
}
public class RegisterVM
{
    [StringLength(100)]
    [EmailAddress]
    [Remote("CheckEmail", "Account", ErrorMessage = "Duplicated {0}.")]
    public string Email { get; set; }

    [StringLength(50, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [StringLength(50, MinimumLength = 6)]
    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string Confirm { get; set; }

    [Required(ErrorMessage = "Residence Plate Number is required")]
    [MaxLength(6, ErrorMessage = "Residence Plate Number can't exceed 6 characters")]
    [Display(Name = "Resident Plate Number")]
    public string ResidencePlateNumber { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    public IFormFile Photo { get; set; }
    public string Recaptcha { get; set; } = string.Empty;
}

public class UpdatePasswordVM
{
    public string ResidencePlateNumber { get; set; }
    [StringLength(50, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string Current { get; set; }

    [StringLength(50, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string New { get; set; }

    [StringLength(50, MinimumLength = 6)]
    [Compare("New")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string Confirm { get; set; }
    public string Recaptcha { get; set; } = string.Empty;
}

public class UpdateProfileVM
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string? PhotoURL { get; set; }
    public string ResidencePlateNumber { get; set; }
    public IFormFile? Photo { get; set; }
}

public class AddMaintenanceTeamVM
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string ResidencePlateNumber { get; set; }
    public IFormFile Photo { get; set; }

}

public class ResetPasswordVM
{
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }
    public string? OTP { get; set; }

    public string? SentOTP { get; set; }
}

public class UserProfileVM
{
    public string Name { get; set; }
    public string ResidencePlateNumber { get; set; }
    public string PhotoURL { get; set; }
    public string Email { get; set; }
}
public class UserListVM
{
    public string Id { get; set; }

    [StringLength(100)]
    [EmailAddress]
    [Remote("CheckEmail", "Account", ErrorMessage = "Duplicated {0}.")]
    public string Email { get; set; }
    public string ResidencePlateNumber { get; set; }

    [Required]
    public string Discriminator { get; set; }
}


public class RatingVM
{
    [MaxLength(10)]
    [RegularExpression(@"RT\d{4}", ErrorMessage = "Invalid {0} format.")]
    public string Id { get; set; }

    [Range(1, 5)]
    [RegularExpression("^[1-5]$", ErrorMessage = "Please select the number between 1 and 5. ")]
    [Required(ErrorMessage = "Rating is required.")]
    public int StarCount { get; set; }

    [StringLength(255)]
    [Required(ErrorMessage = "Text is required.")]
    public string Text { get; set; }

    public DateTime RatingDate { get; set; }

    public string ReportId { get; set; }

    public string UserId { get; set; }

    public List<IFormFile> Photos { get; set; } = new List<IFormFile>();

    //Navigation
    public Report? Report { get; set; }

}

public class RatingUpdateVM
{
    public string Id { get; set; }

    [Range(1, 5)]
    [RegularExpression("^[1-5]$", ErrorMessage = "Please select the number between 1 and 5. ")]
    [Required(ErrorMessage = "Rating is required.")]
    public int StarCount { get; set; }

    [StringLength(255)]
    [Required(ErrorMessage = "Text is required.")]
    public string Text { get; set; }

    public DateTime RatingDate { get; set; }

    //FK

    public string ReportId { get; set; }

    public string UserId { get; set; }

    public List<string>? PhotoURLs { get; set; }

    public List<IFormFile>? Photos { get; set; }

    //Navigation
    public Report? Report { get; set; }

}

public class VendorVM
{

    [StringLength(10)]
    public string Id { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Owner Name is required.")]
    public string OwnerName { get; set; }

    [StringLength(100)]
    [Remote("CheckEmail", "Vendor", AdditionalFields = "originalVendorEmail", ErrorMessage = "Duplicated {0}.")]
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [StringLength(16)]
    [RegularExpression(@"^\+?[1-9]\d{7,14}", ErrorMessage = "Contact number must be a number between 8-15 digit")]
    [Required(ErrorMessage = "Contact Number is required.")]
    public string Contact { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "Company Name is required.")]
    public string CompanyName { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Company Type is required.")]
    public string CompanyType { get; set; }

}
public class FacilitiesVM
{
    [Required(ErrorMessage = "Id is required.")]
    public string Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please select Areas.")]
    [Display(Name = "Areas")]
    public string AreaID { get; set; }

    public string AreaName { get; set; }

    public List<string> PhotoURLs { get; set; } = new List<string>();

}

public class FacilitiesInsertVM
{
    [Required(ErrorMessage = "Id is required.")]
    public string Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please select Areas.")]
    [Display(Name = "Areas")]
    public string AreaID { get; set; }

    public List<IFormFile> Photos { get; set; }
}

public class FacilitiesUpdateVM
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string AreaID { get; set; }

    // Holds existing photo URLs for display purposes
    public List<string> PhotoURLs { get; set; } = new List<string>();

    // For uploading multiple new photos
    public IFormFileCollection? Photos { get; set; }
}

public class MaintenanceTeamVM
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Contact { get; set; }

    public string CompanyName { get; set; }

    public string CompanyType { get; set; }

    public string? ReportTitle { get; set; }

}

public class MaintenanceTeamUpdateVM
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Contact { get; set; }

    public string CompanyName { get; set; }

    public string CompanyType { get; set; }

    public string ReportId { get; set; }

}




