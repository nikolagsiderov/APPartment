using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APPartment.Blazor.Web.Data
{
    public class APPAuthStateProvider : AuthenticationStateProvider
    {
        private ISessionStorageService sessionStorageService;

        public APPAuthStateProvider(ISessionStorageService sessionStorageService)
        {
            this.sessionStorageService = sessionStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userID = await sessionStorageService.GetItemAsync<string>("currentUserID");
            var username = await sessionStorageService.GetItemAsync<string>("currentUsername");
            var homeID = await sessionStorageService.GetItemAsync<string>("currentHomeID");
            var homeName = await sessionStorageService.GetItemAsync<string>("currentHomeName");

            var identity = new ClaimsIdentity();

            if (string.IsNullOrEmpty(userID) == false && string.IsNullOrEmpty(username) == false
                && string.IsNullOrEmpty(homeID) == false && string.IsNullOrEmpty(homeName) == false)
            {
                identity = new ClaimsIdentity(new[]
                    {
                        new Claim("currentUserID", userID), new Claim("currentUsername", username), new Claim("currentHomeID", homeID), new Claim("currentHomeName", homeName)
                    }, "apiauth_type");
            }

            var user = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(user));
        }

        public void ChangeAuthState(string userID, string username, string homeID, string homeName)
        {
            var identity = new ClaimsIdentity();

            if (string.IsNullOrEmpty(userID) == false && string.IsNullOrEmpty(username) == false
                && string.IsNullOrEmpty(homeID) == false && string.IsNullOrEmpty(homeName) == false)
            {
                identity = new ClaimsIdentity(new[]
                    {
                        new Claim("currentUserID", userID), new Claim("currentUsername", username), new Claim("currentHomeID", homeID), new Claim("currentHomeName", homeName)
                    }, "apiauth_type");
            }

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
