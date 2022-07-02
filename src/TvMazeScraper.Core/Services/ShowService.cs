using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TvMazeScraper.Core.ApiResponse;
using TvMazeScraper.Core.Dtos;
using TvMazeScraper.Core.Entities;
using TvMazeScraper.Core.Repositories;

namespace TvMazeScraper.Core
{
    public class ShowService : IShowService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IShowRepository _showRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public ShowService(IHttpClientFactory httpClientFactory, IShowRepository showRepository, IPersonRepository personRepository, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _showRepository = showRepository;
            _personRepository = personRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ShowDto>> GetShows(int pageIndex, int pageSize)
        {
            var result = _mapper.Map<IEnumerable<ShowDto>>(await _showRepository.GetShows(pageIndex, pageSize));
            foreach (var item in result)
                item.Cast = item.Cast.OrderByDescending(p => p.BirthDay);

            return result;
        }
    }
}
