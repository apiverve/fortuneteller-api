using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace APIVerve.API.FortuneTeller
{
    /// <summary>
    /// Query options for the Fortune Teller API
    /// </summary>
    public class FortuneTellerQueryOptions
    {
        /// <summary>
        /// Fortune category: general, love, career, health, wealth, travel
        /// Example: general
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Optional question for personalized fortune (max 500 characters)
        /// Example: What does the future hold for me?
        /// </summary>
        [JsonProperty("question")]
        public string Question { get; set; }
    }
}
