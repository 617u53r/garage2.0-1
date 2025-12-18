
using Garage_2.Models;
using System.ComponentModel.DataAnnotations;
namespace Garage_2.ViewModels;

public class ParkedVehicleCreateVm
{
    [Required]
    public VehicleType Type { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "Registration number can't be more than 10 chars.")]
    public string RegNr { get; set; } = "";

    [Required]
    [StringLength(30)]
    public string Color { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Brand { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Model { get; set; } = "";

    [Range(0, 18, ErrorMessage = "Number of weels must be between 0 and 18.")]
    public int Wheels { get; set; }

}

