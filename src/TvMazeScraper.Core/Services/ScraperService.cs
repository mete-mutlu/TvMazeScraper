using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Core.ApiResponse;
using TvMazeScraper.Core.Dtos;
using TvMazeScraper.Core.Entities;
using TvMazeScraper.Core.Repositories;

namespace TvMazeScraper.Core.Services
{


    public class ScraperService : IScraperService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IShowRepository _showRepository;
        private readonly IPersonRepository _personRepository;

        public ScraperService(IHttpClientFactory httpClientFactory, IShowRepository showRepository, IPersonRepository personRepository)
        {
            _httpClientFactory = httpClientFactory;
            _showRepository = showRepository;
            _personRepository = personRepository;
        }
        public async Task Scrape(CancellationToken cancellationToken)
        {

            var showsFromDb = await _showRepository.GetShows();
            var apiClient = _httpClientFactory.CreateClient("TvMazeApiClient");
            var shows = JsonConvert.DeserializeObject<IEnumerable<ShowApiResponse>>(await CallApi(apiClient, "/shows"));
            foreach (var show in shows)
            {
                var showFromDb = showsFromDb.Where(p => p.Id == show.Id).FirstOrDefault();
                if (showFromDb == null)
                {
                    var casts = JsonConvert.DeserializeObject<IEnumerable<CastRootApiResponse>>(await CallApi(apiClient, $"/shows/{show.Id}/cast"));
                    var newShow = new Show()
                    {
                        Id = show.Id,
                        Name = show.Name,
                        Cast = new List<Cast>()
                    };
                    foreach (var cast in casts)
                    {
                        var samePersonAddedForAnotherCharacter = newShow.Cast.Any(p => p.PersonId == cast.Person.Id);
                        var existsInDb = await _personRepository.Exists(cast.Person.Id);
                        newShow.Cast.Add(
                            new Cast
                            {
                                PersonId = cast.Person.Id,
                                ShowId = newShow.Id,
                                Person = !existsInDb && !samePersonAddedForAnotherCharacter ?
                            new Person { Id = cast.Person.Id, Name = cast.Person.Name, BirthDay = cast.Person.BirthDay } : null
                            });
                    }
                    await _showRepository.AddShow(newShow);
                }
            }
        }

        private static async Task<string> CallApi(HttpClient apiClient, string route)
        {
            var showsResponse = await apiClient.GetAsync(route);
            return await showsResponse.Content.ReadAsStringAsync();
        }
    }








}
