using Garage_2.Models;

namespace Garage_2.ViewModels
{
	public class CheckOutViewModel
	{
		public ParkedVehicle Vehicle { get; set; } = null!;
		public string? Message { get; set; }
	}
}
