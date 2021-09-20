using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Tipple.APIClient.Model
{
    public class ApiClientConfiguration
    {

        public string BaseUrl { get; set; }

        public ApiClientConfiguration(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            BaseUrl = baseUrl;

        }
    }
}
