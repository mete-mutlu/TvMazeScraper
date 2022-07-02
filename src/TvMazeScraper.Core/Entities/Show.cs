
using System.Collections.Generic;

namespace TvMazeScraper.Core.Entities
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Cast> Cast { get; set; }
    }
}
