using AXA_DemoConsoAPI_Front.Models;
using AXA_DemoConsoAPI_Front.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;

namespace AXA_DemoConsoAPI_Front.Pages
{
    public partial class Login
    {
        [Inject]
        public HttpClient _client { get; set; }
        [Inject]
        public IJSRuntime _js { get; set; }

        private string url = "https://localhost:7271/api/";

        [Inject]
        public NavigationManager _nav { get; set; }

        [Inject]
        public IServiceProvider service { get; set; }

        public string ErrorMsg { get; set; }

        public LoginForm MyForm { get; set; }
        public Login()
        {
            MyForm = new LoginForm();
        }

        public async Task Authenticate()
        {
            _client.BaseAddress = new Uri(url);

            string json = JsonSerializer.Serialize(MyForm);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using(HttpResponseMessage response = await _client.PostAsync("auth", content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMsg = response.StatusCode.ToString();
                }

                string jsonresponse = await response.Content.ReadAsStringAsync();

                await _js.InvokeVoidAsync("sessionStorage.setItem", "token", jsonresponse);

                ((MyStateProvider)service.GetService<AuthenticationStateProvider>()).NotifyUserChanged();

                _nav.NavigateTo("/");
            }
        }
    }
}
