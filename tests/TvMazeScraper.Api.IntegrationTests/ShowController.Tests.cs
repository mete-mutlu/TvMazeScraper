using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TvMazeScraper.Core.Dtos;
using Xunit;

namespace Api.IntegrationTests
{
    public class ShowControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;





        public ShowControllerTests(CustomWebApplicationFactory<Program> factory)
        {

            _configuration = factory.Services.GetRequiredService<IConfiguration>();
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });


        }
        [Theory]
        [InlineData(0, 5, 2, 6)]
        [InlineData(0, 1, 1, 3)]
        public async Task Get_ReturnsOK(int pageIndex, int pageSize, int expectedShowCount, int expectedCastCount)
        {
            var response = await _client.GetAsync($"/api/show/{pageIndex}/{pageSize}");
            var content = await response.Content.ReadAsStringAsync();
            var shows = JsonConvert.DeserializeObject<IEnumerable<ShowDto>>(content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedShowCount, shows.Count());
            Assert.Equal(expectedCastCount, shows.SelectMany(p => p.Cast).Count());
            foreach (var show in shows)
                show.Cast.Select(p => p.BirthDay).Should().BeInDescendingOrder();

        }



    }
}
