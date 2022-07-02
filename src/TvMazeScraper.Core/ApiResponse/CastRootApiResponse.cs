using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvMazeScraper.Core.ApiResponse
{

    public class CastRootApiResponse
    {
        public int Id { get; set; }
        public PersonApiResponse Person { get; set; }
    }

}
