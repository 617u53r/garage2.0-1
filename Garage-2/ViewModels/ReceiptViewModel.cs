namespace Garage_2.ViewModels
{
    public class ReceiptViewModel
    {
        public required string RegistrationNumber { get; set; }
        public required string VehicleType { get; set; }
        public required string Color { get; set; }
        public required string Manufacturer { get; set; }
        public required string Model { get; set; }
        public int NumberOfWheels { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public TimeSpan TotalParkedTime { get; set; }
        public decimal Price { get; set; }
    }
}
