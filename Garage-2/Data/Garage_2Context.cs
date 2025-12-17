using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage_2.Models;

namespace Garage_2.Data
{
    public class Garage_2Context : DbContext
    {
        public Garage_2Context (DbContextOptions<Garage_2Context> options)
            : base(options)
        {
        }

        public DbSet<Garage_2.Models.ParkedVehicle> ParkedVehicle { get; set; } = default!;
    }
}
