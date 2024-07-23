using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Libreca.Models;
using Newtonsoft.Json;

namespace Libreca.Services
{
    public class BookSearchClient
    {
        private readonly HttpClient _httpClient;

        public BookSearchClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string query)
        {
            var response = await _httpClient.GetAsync($"api/books/search?query={query}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Book>>(content);
        }
    }

}