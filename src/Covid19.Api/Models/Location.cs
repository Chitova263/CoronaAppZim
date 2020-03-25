namespace Covid19.Api.Models
{
    public class Location
    {
        public string Country { get; set; }
        public string Province { get; set; }
        public GeoPosition GeoPosition { get; set; }
    }

    public class GeoPosition
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}