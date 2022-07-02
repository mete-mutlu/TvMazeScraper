using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvMazeScraper.Core.Entities;

namespace TvMazeScraper.Core.Repositories
{
    public interface IPersonRepository
    {
        Task<bool> Exists(int id);
    }
}
