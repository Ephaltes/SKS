using System;

namespace SKS
{
    public class WeatherForecast
    {
        // this is a example comment 
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}