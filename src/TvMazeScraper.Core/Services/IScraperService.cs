using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TvMazeScraper.Core.Services
{

    public interface IScraperService
    {
        Task Scrape(CancellationToken cancellationToken);
    }

}
