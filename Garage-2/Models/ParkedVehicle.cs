namespace Garage_2.Models
{
	public class ParkedVehicle
	{
		public int Id { get; set; }
		public VehicleType VehicleType { get; set; }
		public string LicensePlate { get; set; }
		public string Color { get; set; }
		public string Manufacturer { get; set; }
		public string Model { get; set; }
		public int NumberOfWheels { get; set; }
		public DateTime CheckInTime { get; set; }


		// If this is null then the vehicle is still parked
		public DateTime? CheckOutTime { get; set; }
	}
}
