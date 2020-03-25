namespace Covid19.Api.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Covid19.Api.Models;
    public interface ICovidAPI
    {
        Task<ReportedCases> GetReportedCasesInLocation(string country);
        Task<IEnumerable<Location>> GetLocations();
    }
}