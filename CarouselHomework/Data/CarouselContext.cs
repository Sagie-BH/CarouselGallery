using CarouselHomework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarouselHomework.Data
{
    public class CarouselContext : DbContext
    {
        public CarouselContext(DbContextOptions<CarouselContext> options) : base(options) { }
        public DbSet<ImageModel> Gallery { get; set; }
    }
}
