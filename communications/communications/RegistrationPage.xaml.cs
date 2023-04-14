using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace communications
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        private UserRepository _userRepository;

        public RegistrationPage()
        {
            InitializeComponent();
            var databaseService = new DatabaseService();
            _userRepository = new UserRepository(databaseService.GetConnection());
        }

        //this is the button, when the user clicks the register button 
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text?.Trim();
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter a username, email, and password to continue.", "OK");
                return;
            }

            // Continue with the registration process
            var user = new User { Username = username, Email = email, Password = password };
            var result = await _userRepository.RegisterUserAsync(user);


            if (result)
            {
                await DisplayAlert("Success", "You have successfully registered.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "This User has already been registered", "OK");
            }
        }
    }
}