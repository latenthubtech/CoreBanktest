using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreBankerApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string Message { get; private set; }
        public List<dynamic> Data { get; private set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            Message = "Period";
            Data = new List<dynamic>();
            Data.Add(Message);

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7138/api/customers/1234567890"); // Your API URL
            var content = await response.Content.ReadAsStringAsync();

            // Process the content
            Console.WriteLine(content);
        }
    }
}
