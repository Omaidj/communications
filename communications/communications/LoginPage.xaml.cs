using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace communications
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private UserRepository _userRepository;

        public LoginPage()
        {
            InitializeComponent();
            var databaseService = new DatabaseService();
            _userRepository = new UserRepository(databaseService.GetConnection());
            
            //this removes the back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var user = await _userRepository.AuthenticateUserAsync(UsernameEntry.Text, PasswordEntry.Text);

            if (user != null)
            {
                // Navigate to the main page and pass the current user's ID
                await Navigation.PushAsync(new MainPage(user.Id));
            }
            else
            {
                await DisplayAlert("Sorry:(", "That is the Inccorrect Username and Password.", "OK");
            }
        }
        //forgot password 
        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text;
            if (!string.IsNullOrWhiteSpace(username))
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);

                if (user != null)
                {
                    await DisplayAlert("Forgot Password", $"Your password is: {user.Password}", "Close");
                }
                else
                {
                    await DisplayAlert("Error", "Username not found.", "Close");
                }
            }
            else
            {
                await DisplayAlert("Error", "Please enter your username.", "Close");
            }
        }

        //register button
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Navigate to the registration page
            await Navigation.PushAsync(new RegistrationPage());
        }
    }
}