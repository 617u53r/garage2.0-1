using Garage_2.Models;
using Garage_2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Garage_2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Receipt(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(v => v.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var checkoutTime = DateTime.Now;

            var vm = new ReceiptViewModel
            {
                RegistrationNumber = vehicle.LicensePlate,
                VehicleType = vehicle.VehicleType.ToString(),
                CheckInTime = vehicle.CheckInTime,
                CheckOutTime = checkoutTime,
                TotalParkedTime = checkoutTime - vehicle.CheckInTime,
                Price = CalculatePrice(vehicle.CheckInTime, checkoutTime)
            };

            return View(vm);
        }
        private static List<ParkedVehicle> _vehicles = new List<ParkedVehicle>
        {
            new ParkedVehicle
            {
                Id = 1,
                LicensePlate = "ABC123",
                VehicleType = VehicleType.Car,
                Color = "Red",
                Manufacturer = "Toyota",
                Model = "Corolla",
                NumberOfWheels = 4,
                CheckInTime = DateTime.Now.AddHours(-2) // 2 hours ago
            },
            new ParkedVehicle
            {
                Id = 2,
                LicensePlate = "XYZ789",
                VehicleType = VehicleType.Motorcycle,
                Color = "Blue",
                Manufacturer = "Honda",
                Model = "CB500",
                NumberOfWheels = 2,
                CheckInTime = DateTime.Now.AddMinutes(-90) // 1.5 hours ago
            }
        };

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private decimal CalculatePrice(DateTime checkIn, DateTime checkOut)
        {
            var totalHours = Math.Ceiling((checkOut - checkIn).TotalHours);
            return (decimal)totalHours * 20; // example: 20 SEK/hour
        }
    }
}
