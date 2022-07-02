using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvMazeScraper.Core.Entities;

namespace TvMazeScraper.Infrastructure.EntityFramework
{
    public class ScraperDbContext : DbContext
    {
        public ScraperDbContext(DbContextOptions<ScraperDbContext> options)
      : base(options)
        {
        }


        public DbSet<Show> Shows { get; set; }

        public DbSet<Cast> Casts { get; set; }

        public DbSet<Person> Persons { get; set; }
    }
   
}
