using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvMazeScraper.Core.Entities;
using TvMazeScraper.Core.Repositories;

namespace TvMazeScraper.Infrastructure.EntityFramework.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ScraperDbContext _context;

        public PersonRepository(ScraperDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Exists(int id)
        {
            return await _context.Persons.FirstOrDefaultAsync(p => p.Id == id) != null;
        }
    }
}
