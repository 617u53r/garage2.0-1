using Garage_2.Data;
using Garage_2.Models;
using Garage_2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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



		// GET: ParkedVehicles
		public async Task<IActionResult> Index()
        {
            return View(await _context.ParkedVehicle.ToListAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleType,LicensePlate,Color,Manufacturer,Model,NumberOfWheels,CheckInTime,CheckOutTime")] ParkedVehicle parkedVehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkedVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parkedVehicle);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleType,LicensePlate,Color,Manufacturer,Model,NumberOfWheels,CheckInTime,CheckOutTime")] ParkedVehicle parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkedVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkedVehicleExists(parkedVehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(parkedVehicle);
        }

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

            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle != null)
            {
                _context.ParkedVehicle.Remove(parkedVehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }
    }
}
