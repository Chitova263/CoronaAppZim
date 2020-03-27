namespace Covid19.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Covid19.Api.Services;
    using Covid19.Api.Services.NewsService;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class Covid19Controller : ControllerBase
    {
        private readonly ICovid19ApiService _covid19ApiService;
        private readonly INewsService _newsService;

        public Covid19Controller(ICovid19ApiService covid19ApiService, INewsService newsService)
        {
            _newsService = newsService ?? throw new System.ArgumentNullException(nameof(newsService));
            _covid19ApiService = covid19ApiService ?? throw new System.ArgumentNullException(nameof(covid19ApiService));
        }

        // GET: api/[controller]/latest?country=""
        [HttpGet]
        [Route("latest")]
        public async Task<ActionResult> GetReportedCasesInLocation([FromQuery] string country)
        {
            var cases = await _covid19ApiService.GetReportedCasesInLocation(country);

            if (cases == null)
                return BadRequest();

            return Ok(cases);
        }

        // GET: api/[controller]/locations
        [HttpGet]
        [Route("locations")]
        public async Task<ActionResult> GetLocations()
        {
            var locations = await _covid19ApiService.GetLocations();

            if (!locations.Any())
            {
                return BadRequest();
            }

            return Ok(locations);
        }
        
        [HttpGet]
        [Route("news")]
        public async Task<ActionResult> GetNews([FromQuery] string query)
        {
            var response = await _newsService.GetAllNews(query);
            if(response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

    }
}