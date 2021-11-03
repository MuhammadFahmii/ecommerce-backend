// ------------------------------------------------------------------------------------
// WeatherForecast.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Application.WeatherForecasts.Queries.GetWeatherForecasts
{
    /// <summary>
    /// WeatherForecast
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Gets or sets date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets temperatureC
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Gets temperatureF
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Gets or sets summary
        /// </summary>
        public string Summary { get; set; }
    }
}
