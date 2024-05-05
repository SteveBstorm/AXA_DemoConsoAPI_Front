using AXA_DemoConsoAPI_Front.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AXA_DemoConsoAPI_Front.Pages
{
    public partial class Index
    {
        [Inject]
        public HttpClient _client { get; set; }
        [Inject]
        public IJSRuntime _js { get; set; }
        private string url = "https://localhost:7271/api/";
        
        List<Client> clients = new List<Client>();
        protected override async Task OnInitializedAsync()
        {
            //_client.BaseAddress = new Uri(url);
            string token = await _js.InvokeAsync<string>("localStorage.getItem", "token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using(HttpResponseMessage message = await _client.GetAsync("client"))
            {
                if(message.IsSuccessStatusCode)
                {
                    string json = await message.Content.ReadAsStringAsync();
                    Console.WriteLine(json);
                    clients = JsonSerializer.Deserialize<List<Client>>(json);
                }
            }
        }
    }
}
