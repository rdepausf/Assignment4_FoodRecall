using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Assignment4_FoodRecall.Models;
using Assignment4_FoodRecall.ModelDto;

namespace Assignment4_FoodRecall.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Results> FoodRecall { get; set; }
        
    }
}