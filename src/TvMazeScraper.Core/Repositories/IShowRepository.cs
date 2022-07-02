using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvMazeScraper.Core.Entities;

namespace TvMazeScraper.Core.Repositories
{
    public interface IShowRepository
    {
        Task<IEnumerable<Show>> GetShows(int pageIndex = 0, int? pageSize = null);

        Task AddShow(Show show);
    }
}
