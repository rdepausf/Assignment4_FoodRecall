using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FoodRecall_Group11.Models;
using FoodRecall_Group11.ModelDto;

namespace FoodRecall_Group11.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Results> FoodRecall { get; set; }
        
    }
}