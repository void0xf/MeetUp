using System.Diagnostics;
using Client.Maui.Api.Auth;
using Client.Maui.Api.Users;
using Client.Maui.Store;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;

namespace Client.Maui.ViewModels;

public partial class SignInPageViewModel : ObservableObject
{
    private readonly IAuth _authApi;
    private readonly IUserApi _userApi;
    private readonly UserStore _userStore;

    public SignInPageViewModel(IAuth userIdentityApi, IUserApi usersInfoApi)
    {
        _authApi = userIdentityApi;
        _userApi = usersInfoApi;

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
            grant_type = "password",
            username = $"alice",
            password = $"Pass123$",
            client_id = "postman",
            client_secret = "NotASecret",
            scope = "meetEventApp openid profile"
        };

        try
        {
            var response = await _authApi.Execute(authTokenRequest);

            if (response.IsSuccessStatusCode)
            {
                var accessToken = response.Content.access_token;
                if (accessToken == null)
                {
                    ErrorMessage = "Invalid Login/Password";
                    return;
                }

                _userStore.Username = authTokenRequest.username;
                _userStore.AccessToken = response.Content.access_token;

                Username = string.Empty;
                Password = string.Empty;
                ErrorMessage = string.Empty;

                // Log the token being used for authorization
                Debug.WriteLine($"Using Access Token: Bearer {accessToken}");

                var fetchedUser = await _userApi.GetUserInfo(
                    _userStore.Username,
                    $"Bearer {_userStore.AccessToken}"
                );
                if (fetchedUser != null)
                {
                    await Shell.Current.GoToAsync("//App");
                }
                ErrorMessage = "Couldn't retrieve user info";

                Debug.WriteLine($"Fetched User Info: {fetchedUser}");
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
        catch (Exception ex)
        {
            Debug.WriteLine($"Unexpected Error: {ex.Message}");
            ErrorMessage = "An unexpected error occurred. Please try again.";
        }
    }
}
