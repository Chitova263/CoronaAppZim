namespace Covid19.Api.Models
{
    using System;
    
    public class ReportedCases
    {
        public string Country { get; set; }
        public string Province { get; set; }
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}