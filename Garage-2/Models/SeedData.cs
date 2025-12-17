using Garage_2.Data;
using Microsoft.EntityFrameworkCore;

namespace Garage_2.Models
{
	public static class SeedData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using (var context = new Garage_2Context(
				serviceProvider.GetRequiredService<
					DbContextOptions<Garage_2Context>>()))
			{
				// Look for any parked vehicles.
				if (context.ParkedVehicle.Any())
				{
					return;   // DB has been seeded
				}
				context.ParkedVehicle.AddRange(
					new ParkedVehicle
					{
						VehicleType = VehicleType.Car,
						LicensePlate = "ABC123",
						Color = "Red",
						Manufacturer = "Toyota",
						Model = "Corolla",
						NumberOfWheels = 4,
						CheckInTime = DateTime.Now.AddHours(-2)
					},
					new ParkedVehicle
					{
						VehicleType = VehicleType.Motorcycle,
						LicensePlate = "XYZ789",
						Color = "Blue",
						Manufacturer = "Honda",
						Model = "CBR500R",
						NumberOfWheels = 2,
						CheckInTime = DateTime.Now.AddHours(-1)
					},
					new ParkedVehicle
					{
						VehicleType = VehicleType.Truck,
						LicensePlate = "LMN456",
						Color = "White",
						Manufacturer = "Ford",
						Model = "F-150",
						NumberOfWheels = 4,
						CheckInTime = DateTime.Now.AddHours(-3)
					},
					new ParkedVehicle
					{
						VehicleType = VehicleType.Bus,
						LicensePlate = "BUS321",
						Color = "Yellow",
						Manufacturer = "Mercedes-Benz",
						Model = "Citaro",
						NumberOfWheels = 6,
						CheckInTime = DateTime.Now.AddHours(-4)
					},
					new ParkedVehicle
					{
						VehicleType = VehicleType.Car,
						LicensePlate = "DEF456",
						Color = "Black",
						Manufacturer = "BMW",
						Model = "3 Series",
						NumberOfWheels = 4,
						CheckInTime = DateTime.Now.AddHours(-5)
					},
					new ParkedVehicle
					{
						VehicleType = VehicleType.Motorcycle,
						LicensePlate = "GHI789",
						Color = "Green",
						Manufacturer = "Yamaha",
						Model = "MT-07",
						NumberOfWheels = 2,
						CheckInTime = DateTime.Now.AddHours(-6)
					}

				);
				context.SaveChanges();
			}
		}
	}

}
