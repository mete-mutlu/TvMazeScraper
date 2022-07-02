using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvMazeScraper.Core.Entities
{
    public class Cast
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public int ShowId { get; set; }

        public Show Show { get; set; }
    }

   
}
