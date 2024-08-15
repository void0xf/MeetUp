using System.Collections.ObjectModel;
using Client.Maui.Api.MeetEvent;
using Client.Maui.Store;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Maui.ViewModels
{
    public partial class CreateEventViewModel : ObservableObject
    {
        private readonly IMeetEventApi _eventApi;
        private readonly IMeetEventApi _meetEventApi;
        private readonly UserStore _userStore;

        public CreateEventViewModel(IMeetEventApi eventApi)
        {
            _eventApi = eventApi;
            Images = new ObservableCollection<string>();
            VisibilityOptions = new List<string> { "Public", "Private" };
            _userStore = App.Current.Handler.MauiContext.Services.GetService<UserStore>();
        }

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private DateTime eventStartDate = DateTime.UtcNow.Date;

        [ObservableProperty]
        private TimeSpan eventStartTime = DateTime.UtcNow.TimeOfDay;

        [ObservableProperty]
        private DateTime eventEndDate = DateTime.UtcNow.Date;

        [ObservableProperty]
        private TimeSpan eventEndTime = DateTime.UtcNow.TimeOfDay;

        [ObservableProperty]
        private string location;

        [ObservableProperty]
        private string author;

        [ObservableProperty]
        private string visibility;

        [ObservableProperty]
        private ObservableCollection<string> images;

        [ObservableProperty]
        private List<string> visibilityOptions;

        [RelayCommand]
        private void AddImage()
        {
            // Implement image selection logic here
            // For now, we'll just add a placeholder
            Images.Add($"Image {Images.Count + 1}");
        }

        [RelayCommand]
        private async Task CreateEvent()
        {
            var newEvent = new CreateEventDto
            {
                Title = Title,
                Description = Description,
                EventStartDate = eventStartDate.Date + eventStartTime,
                EventEndDate = eventEndDate.Date + eventEndTime,
                Location = Location,
                Author = Author,
                Visibility = Visibility,
                Images = Images.ToList()
            };

            try
            {
                var response = await _eventApi.CreateEventAsync(
                    newEvent,
                    $"Bearer {_userStore.AccessToken}"
                );
                // Handle successful creation (e.g., show a success message, navigate back)
                await Shell.Current.DisplayAlert("Success", "Event created successfully", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                // Handle error (e.g., show an error message)
                await Shell.Current.DisplayAlert(
                    "Error",
                    $"Failed to create event: {ex.Message}",
                    "OK"
                );
            }
        }
    }
}
