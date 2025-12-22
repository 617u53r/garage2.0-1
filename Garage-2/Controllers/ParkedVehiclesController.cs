using Garage_2.Data;
using Garage_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garage_2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage_2.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly Garage_2Context _context;

        public ParkedVehiclesController(Garage_2Context context)
        {
            _context = context;
        }



		public async Task<IActionResult> Index(string? licensePlate)
		{
			var vehicles = _context.ParkedVehicle
				.Where(v => v.CheckOutTime == null);

			if (!string.IsNullOrWhiteSpace(licensePlate))
			{
				licensePlate = licensePlate.Trim().ToUpper();

				vehicles = vehicles.Where(v =>
					v.LicensePlate.Contains(licensePlate));
			}

			return View(await vehicles.ToListAsync());
		}






		// GET: ParkedVehicles/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Receipt/5
        public async Task<IActionResult> Receipt(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            var checkOutTime = DateTime.Now; // set at checkout
            vehicle.CheckOutTime = checkOutTime;
            // Calculate total parked time
            var totalTime = checkOutTime - vehicle.CheckInTime;

            // Round total hours to nearest half-hour
            var totalHours = totalTime.TotalHours;
            var roundedHours = Math.Round(totalHours * 2, MidpointRounding.AwayFromZero) / 2; // nearest 0.5 hr

            // Calculate price based on rounded hours
            var price = CalculatePriceFromHours(roundedHours);

            var vm = new ReceiptViewModel
            {
                RegistrationNumber = vehicle.LicensePlate,
                VehicleType = vehicle.VehicleType.ToString(),
                Color = vehicle.Color,
                Manufacturer = vehicle.Manufacturer,
                Model = vehicle.Model,
                NumberOfWheels = vehicle.NumberOfWheels,
                CheckInTime = vehicle.CheckInTime,
                CheckOutTime = checkOutTime,
                TotalParkedTime = TimeSpan.FromHours(roundedHours),
                Price = price
            };

            // REMOVE after checkout
            _context.ParkedVehicle.Remove(vehicle);
            await _context.SaveChangesAsync();

            return View(vm);
        }

        private decimal CalculatePriceFromHours(double hours)
        {
            decimal ratePerHour = 20m; // 20kr per hour
            return (decimal)hours * ratePerHour;
        }

        // GET: ParkedVehicles/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ParkedVehicleCreateVm());
        }


        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(ParkedVehicleCreateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var reg = vm.RegNr.Trim().ToUpperInvariant();

            if (await _context.ParkedVehicle.AnyAsync(v => v.LicensePlate == reg))
            {
                ModelState.AddModelError(nameof(vm.RegNr), "Registration number is already used.");
                return View(vm);
            }

            var vehicle = new ParkedVehicle
            {
                VehicleType = vm.Type,
                LicensePlate = reg,
                Color = vm.Color.Trim(),
                Manufacturer = vm.Brand.Trim(),
                Model = vm.Model.Trim(),
                NumberOfWheels = vm.Wheels,
                CheckInTime = DateTime.Now, 
                CheckOutTime = null
            };

            _context.ParkedVehicle.Add(vehicle);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Vehicle {vehicle.LicensePlate} Parked successfully.";
            return RedirectToAction(nameof(Index));


        }


        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }
            return View(parkedVehicle);
        }

		// POST: ParkedVehicles/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleType,LicensePlate,Color,Manufacturer,Model,NumberOfWheels")] ParkedVehicle parkedVehicle)
		{
			if (id != parkedVehicle.Id)
			{
				return NotFound();
			}

			// Retrieve the existing vehicle from the database
			var dbVehicle = await _context.ParkedVehicle.FindAsync(id);

			if (dbVehicle == null)
				return NotFound();

			// Does the licence plate already exist on another vehicle?
			bool licenseExists = await _context.ParkedVehicle.AnyAsync(v => v.LicensePlate == parkedVehicle.LicensePlate && v.Id != parkedVehicle.Id);

			if (licenseExists)
			{
				// set the model state to invalid
				ModelState.AddModelError(nameof(ParkedVehicle.LicensePlate), "A vehicle with this license plate already exists.");
			}

			if (!ModelState.IsValid)
			{
				return View(parkedVehicle);
			}

			try
			{
				// Update all fields except CheckInTime and CheckOutTime
				dbVehicle.VehicleType = parkedVehicle.VehicleType;
				dbVehicle.LicensePlate = parkedVehicle.LicensePlate;
				dbVehicle.Color = parkedVehicle.Color;
				dbVehicle.Manufacturer = parkedVehicle.Manufacturer;
				dbVehicle.Model = parkedVehicle.Model;
				dbVehicle.NumberOfWheels = parkedVehicle.NumberOfWheels;
				await _context.SaveChangesAsync();
				TempData["Success"] = "Vehicle was successfully updated.";

			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ParkedVehicleExists(parkedVehicle.Id))
				{
					return NotFound();
				}
				throw;
			}
            TempData["Success"] = $"Vehicle {parkedVehicle.LicensePlate} edited successfully.";
            return RedirectToAction(nameof(Index));
        }

		// GET: ParkedVehicles/Delete/5
		// GET: ParkedVehicles/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var parkedVehicle = await _context.ParkedVehicle
				.FirstOrDefaultAsync(m => m.Id == id);
			if (parkedVehicle == null)
			{
				return NotFound();
			}
			var vm = new CheckOutViewModel
			{
				Vehicle = parkedVehicle
			};

			return View(vm);
		}

		// POST: ParkedVehicles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);

			if (parkedVehicle == null)
				return NotFound();


			var vm = new CheckOutViewModel
			{
				Vehicle = parkedVehicle
			};


			// check if the vehicle is already checked out
			if (parkedVehicle.CheckOutTime != null)
			{
				ModelState.AddModelError("", "Vehicle is already checked out.");
				return View(vm);
			}

			// set the CheckOutTime to local time
			parkedVehicle.CheckOutTime = DateTime.Now;

			var duration = parkedVehicle.CheckOutTime.Value - parkedVehicle.CheckInTime;

			int h = duration.Hours;
			int m = duration.Minutes;

			if (h > 0)
			{
				vm.Message = $"Vehicle checked out. Duration: {duration.Hours} hours and {duration.Minutes} minutes.";
			}
			else
			{
				vm.Message = $"Vehicle checked out. Duration: {duration.Minutes} minutes.";
			}


			await _context.SaveChangesAsync();

            TempData["Success"] = $"Vehicle {parkedVehicle.LicensePlate} checked out successfully.";
            return RedirectToAction(nameof(Index));

        }


		private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }



		public async Task<IActionResult> Search(string? licensePlate)
		{
			var vehicles = _context.ParkedVehicle.AsQueryable();

			if (!string.IsNullOrWhiteSpace(licensePlate))
			{
				vehicles = vehicles.Where(v =>
					v.LicensePlate.Contains(licensePlate));
			}

			return View(await vehicles.ToListAsync());
		}









	}
}
