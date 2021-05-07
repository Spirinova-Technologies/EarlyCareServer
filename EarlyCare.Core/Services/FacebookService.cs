using EarlyCare.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace EarlyCare.Core.Services
{
   public class FacebookService: IFacebookService
    {
        private HttpClient httpClient;

        public FacebookService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("earlycare");
        }

        public async Task GetFacebookFeeds()
        {
          //  var response = await httpClient.SendAsync();

          //  response.EnsureSuccessStatusCode();
        }

    }
}
