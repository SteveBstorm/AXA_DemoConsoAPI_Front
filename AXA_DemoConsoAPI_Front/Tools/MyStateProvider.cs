using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AXA_DemoConsoAPI_Front.Tools
{
    public class MyStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        private readonly NavigationManager _nav;
        public MyStateProvider(IJSRuntime js, NavigationManager nav)
        {
            _js = js;
            _nav = nav;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _js.InvokeAsync<string>("localStorage.getItem", "token");
            Console.WriteLine(token);
            if (!string.IsNullOrEmpty(token))
            {
                JwtSecurityToken jwt = new JwtSecurityToken(token);
                //if (bool.Parse(jwt.Claims.First(x => x.Type == ClaimTypes.Expiration).Value) == true)
                //{
                //    _nav.NavigateTo("login");

                //}

                List<Claim> claims = new List<Claim>();

                foreach (Claim claim in jwt.Claims)
                {
                    claims.Add(claim);
                }

                ClaimsIdentity currentUser = new ClaimsIdentity(claims, "toto");
                
                
                //Console.WriteLine(user);

                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(currentUser)));
            }

            //Console.WriteLine(new ClaimsPrincipal(new ClaimsIdentity()).Identity.IsAuthenticated);
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));


        }

        public void NotifyUserChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
