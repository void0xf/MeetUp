using System.Net;
using Client.Maui.Api.Auth;
using Client.Maui.Api.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Maui.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IAuth _userIdentityApi;
        private readonly IUserApi _usersInfoApi;

        public SignUpViewModel(IAuth userIdentityApi, IUserApi usersInfoApi)
        {
            _userIdentityApi = userIdentityApi;
            _usersInfoApi = usersInfoApi;
        }

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string fullname;

        [RelayCommand]
        private async Task SignUp()
        {
            var userIdentity = new IdentityUserInfo();
            userIdentity.Username = username;
            userIdentity.Email = email;
            userIdentity.Password = password;
            var resposne = await _userIdentityApi.CreateNewIdentity(userIdentity);
            if (resposne.StatusCode == HttpStatusCode.BadRequest)
            {
                await Application.Current.MainPage.DisplayAlert("Sign Up", "Wrong forms!", "OK");
            }

            if (resposne.StatusCode == HttpStatusCode.OK)
            {
                var authTokenRequest = new AuthTokenRequest
                {
                    grant_type = "password",
                    username = username,
                    password = password,
                    client_id = "postman",
                    client_secret = "NotASecret",
                    scope = "meetEventApp openid profile"
                };

                var userInfo = new DtoUserInfo();
                userInfo.Username = username;
                userInfo.Fullname = fullname;
                userInfo.Description = description;

                var response = await _userIdentityApi.Execute(authTokenRequest);
                var token = $"Bearer {response.Content.access_token}";

                var res = await _usersInfoApi.CreateUser(userInfo, token);

                if (res.StatusCode == HttpStatusCode.Created)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Sign Up",
                        "Sign up successful!",
                        "OK"
                    );
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Sign Up",
                        "Something Went Wrong",
                        "OK"
                    );
                }
            }
        }
    }
}
