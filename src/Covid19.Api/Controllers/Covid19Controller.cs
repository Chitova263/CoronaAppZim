namespace Covid19.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Covid19.Api.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class Covid19Controller: ControllerBase
    {
        private readonly ICovidAPI _covidAPI;

        public Covid19Controller(ICovidAPI covidAPI)
        {
            _covidAPI = covidAPI ?? throw new System.ArgumentNullException(nameof(covidAPI));
        }

        [HttpGet]
        [Route("cases")]
        public async Task<ActionResult> GetReportedCasesInLocation([FromQuery] string country)
        {
            var cases = await _covidAPI.GetReportedCasesInLocation(country);
            
            if(cases == null)
                return BadRequest();

            return Ok(cases);
        }

        [HttpGet]
        [Route("locations")]
        public async Task<ActionResult> GetLocations()
        {
            var locations = await _covidAPI.GetLocations();

            if(!locations.Any())
            {
                return BadRequest();
            }
            
            return Ok(locations);
        }
    }
}