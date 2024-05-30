using Client.Maui.Api.Users;
using Client.Maui.Store;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using System.Diagnostics;
using System.Text.Json;

namespace Client.Maui.ViewModels
{
    public partial class SignInPageViewModel : ObservableObject
    {
        private readonly IAuth _authApi;
        private readonly UserStore _userStore;

        public SignInPageViewModel()
        {
            _authApi = RestService.For<IAuth>("http://10.0.2.2:5000");
            _userStore = App.Current.Handler.MauiContext.Services.GetService<UserStore>();
        }

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [RelayCommand]
        public async Task SignIn()
        {
            var authTokenRequest = new AuthTokenRequest
            {
                GrantType = "password",
                Username = "alice",
                Password = "Pass123$",
                ClientId = "postman",
                ClientSecret = "NotASecret",
                Scope = "meetEventApp openid profile"
            };

            try
            {
                var response = await _authApi.Execute(authTokenRequest);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonDoc = JsonDocument.Parse(jsonResponse);
                    var accessToken = jsonDoc.RootElement.GetProperty("access_token").GetString();
                    Debug.WriteLine($"Access Token: {accessToken}");
                    if (accessToken == null)
                    {
                        ErrorMessage = "Invalid Login/Password";
                        return;
                    }

                    // Store the Username and AccessToken in UserStore
                    _userStore.Username = authTokenRequest.Username;
                    _userStore.AccessToken = accessToken;

                    // Clear input fields and error message
                    Username = string.Empty;
                    Password = string.Empty;
                    ErrorMessage = string.Empty;


                }
                else
                {
                    Debug.WriteLine($"Request failed with status code: {response.StatusCode}");
                    Debug.WriteLine($"Reason: {response.ReasonPhrase}");
                    ErrorMessage = "Login failed. Please try again.";
                }
            }
            catch (ApiException apiEx)
            {
                Debug.WriteLine($"API Error: {apiEx.StatusCode}");
                Debug.WriteLine($"Content: {apiEx.Content}");
                ErrorMessage = "An error occurred during login. Please try again.";
            }
        }
    }
}
