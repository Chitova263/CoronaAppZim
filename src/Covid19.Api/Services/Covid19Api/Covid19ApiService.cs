namespace Covid19.Api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Covid19.Api.Models;
    using Microsoft.Extensions.Logging;
    using TinyCsvParser.Tokenizer.RFC4180;

    public class Covid19ApiService: ICovid19ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<Covid19ApiService> _logger;
        public static RFC4180Tokenizer Tokenizer => new RFC4180Tokenizer(new Options('"', '\\', ','));
        public Covid19ApiService(IHttpClientFactory httpClientFactory, ILogger<Covid19ApiService> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Location>> GetLocations()
        {
            IEnumerable<Location> locations = new List<Location>();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                //serialize to string
                var content = await response.Content.ReadAsStringAsync();
                locations = content
                    .Split(new[] { '\n' }, StringSplitOptions.None)
                    .Skip(1)
                    .SkipLast(1)
                    .Select(x => Tokenizer.Tokenize(x))
                    .Select(x => new Location
                    {
                        Country = x[1],
                        Province = x[0],
                        Latitude = Double.Parse(x[2].Trim()),
                        Longitude =  Double.Parse(x[3].Trim())
                    });
            }
            return locations;
        }

        public async Task<ReportedCases> GetReportedCasesInLocation(string country)
        {
            _logger.LogInformation(country);
            var cases = new ReportedCases();

            var confirmedCasesRequest = new HttpRequestMessage(HttpMethod.Get, "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv");
            var deathsRequest = new HttpRequestMessage(HttpMethod.Get, "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_deaths_global.csv");

            var httpClient = _httpClientFactory.CreateClient();

            //fix this to run Tasks in parallel
            var confirmedCasesResponse = await httpClient.SendAsync(confirmedCasesRequest);
            var deathsResponse = await httpClient.SendAsync(deathsRequest);

            if(confirmedCasesResponse.IsSuccessStatusCode && deathsResponse.IsSuccessStatusCode)
            {
                var confirmed = await confirmedCasesResponse.Content.ReadAsStringAsync();
                var deaths = await deathsResponse.Content.ReadAsStringAsync();

                var date = confirmed
                    .Split(new []{"\n"}, StringSplitOptions.None)
                    .First()
                    .Split(",")
                    .Last()
                    .Trim();
                    
                    

                var result = confirmed
                    .Split(new []{"\n"}, StringSplitOptions.None)
                    .FirstOrDefault(x => x.Contains(country))
                    .Trim()
                    .Split(",");

                var res = deaths
                    .Split(new []{"\n"}, StringSplitOptions.None)
                    .FirstOrDefault(x => x.Contains(country))
                    .Trim()
                    .Split(",");

                cases.Confirmed = Int32.Parse(result.Last());
                cases.Country = result[1];
                cases.Province = result[0];
                cases.Latitude = Double.Parse(result[2]);
                cases.Longitude = Double.Parse(result[3]);
                cases.Deaths = Int32.Parse(res.Last());  
                cases.TimeStamp = DateTime.Parse(date, CultureInfo.InvariantCulture).Date;                  
            } 
            else 
            {
                return null;
            }
            return cases;
        }
    }
}