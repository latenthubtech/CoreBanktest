using System.Text.Json;
using System;
using CoreBankerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreBankerApp.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public string AccountNumber { get; set; }

        private readonly ILogger<IndexModel> _logger;
        public string Message { get; private set; }
        public List<dynamic> Data { get; private set; } = new List<dynamic>();
        public BaseResponse<Customer> Customer { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            
            
        }

        public async Task OnPostSearchAsync()
        {

            var client = _httpClientFactory.CreateClient("ApiWithBasicAuth");
            var response = await client.GetAsync($"http://localhost:3000/api/customers/{AccountNumber}"); // Your API URL
            var content = await response.Content.ReadAsStringAsync();

            Customer = JsonSerializer.Deserialize<BaseResponse<Customer>>(content);

            HttpContext.Session.SetString("Customer", JsonSerializer.Serialize(Customer));

            // Process the content
            Console.WriteLine(content);

           
        }


        public async Task OnPostSaveAsync()
        {

            var SubmittedData = new Dictionary<string, string>();

            var accNumber = AccountNumber;

            var userInfoJson = HttpContext.Session.GetString("Customer");

            Customer = JsonSerializer.Deserialize<BaseResponse<Customer>>(userInfoJson);

            foreach (var field in Customer.data.industry.industryTypes)
            {
                string input = Request.Form[$"field_{field.industryTypeId}"];
                if (string.IsNullOrEmpty(input))
                {
                    ModelState.AddModelError($"field_{field.industryTypeId}", $"{field.label} is required.");
                }
                else
                {
                    SubmittedData.Add(field.label, input);

                }
            }

            Console.WriteLine(SubmittedData);


        }
    }
}
