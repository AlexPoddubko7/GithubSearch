using DataAccess.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace DataAccess.Repositories
{
    public class GithubRepository: IGithubRepository
    {
        public T Get<T>(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Awesome-Octocat-App");
                var response = client.GetAsync(url).Result;
                if(response.StatusCode != HttpStatusCode.NotFound)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(content);
                }                
            }
            return default(T);
        }
    }
}
