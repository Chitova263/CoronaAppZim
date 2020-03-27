namespace Covid19.Api.Models
{
    using System;
    
    public class ReportedCases
    {
        public string Country { get; set; }
        public string Province { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public string TimeStamp { get; set; }

    }
}