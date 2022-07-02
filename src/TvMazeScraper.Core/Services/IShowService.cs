using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Core.Dtos;

namespace TvMazeScraper.Core
{
    public interface IShowService
    {
        Task<IEnumerable<ShowDto>> GetShows(int pageIndex, int pageSize);
    }
}