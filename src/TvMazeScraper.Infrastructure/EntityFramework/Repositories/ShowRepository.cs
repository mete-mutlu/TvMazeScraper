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
    public class ShowRepository : IShowRepository
    {
        private readonly ScraperDbContext _dbContext;

        public ShowRepository(ScraperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddShow(Show show)
        {
            _dbContext.Add(show);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Show>> GetShows(int pageIndex=0, int? pageSize = null)
        {
            var pageSizeInt = pageSize != null? pageSize.Value : 0;
            var query = _dbContext.Shows.Skip(pageIndex * pageSizeInt);
            query = pageSize.HasValue ? query.Take(pageSizeInt) : query;
            var list = await query.Include(p=>p.Cast).Include("Cast.Person").ToListAsync();
            return list;
        }
    }
}
