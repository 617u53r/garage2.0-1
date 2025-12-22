using System.ComponentModel.DataAnnotations;

namespace Garage_2.Models
{
	public class ParkedVehicle
	{
		public int Id { get; set; }


		[Required]
		[Display(Name = "Vehicle Type")]

		public VehicleType VehicleType { get; set; }



		[Required]
		[StringLength(20, MinimumLength = 1)]
		[Display(Name = "License Plate")]

		public string LicensePlate { get; set; } = string.Empty;


		[Required]
		[StringLength(20, MinimumLength = 1)]
		public string Color { get; set; } = string.Empty;


		[Required]
		[StringLength(20, MinimumLength = 1)]
		public string Manufacturer { get; set; } = string.Empty;


		[Required]
		[StringLength(20, MinimumLength = 1)]
		public string Model { get; set; } = string.Empty;




		[Range(1, 18, ErrorMessage = "Number of wheels must be between 1 and 18.")]
		[Display(Name = "Wheels")]
		public int NumberOfWheels { get; set; }


		[Display(Name = "Parked started")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
		public DateTime CheckInTime { get; set; }


		[Display(Name = "Parked ended")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
		public DateTime? CheckOutTime { get; set; }
	}
}
