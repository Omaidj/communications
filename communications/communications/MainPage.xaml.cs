using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace communications
{
    public partial class MainPage : ContentPage
    {
        private UserRepository _userRepository;
        private List<User> _users;
        private int _currentUserId;

        public MainPage(int currentUserId)
        {
            InitializeComponent();
            var databaseService = new DatabaseService();
            _userRepository = new UserRepository(databaseService.GetConnection());
            _currentUserId = currentUserId;
            LoadUsers();

            // Hide the back button
            NavigationPage.SetHasBackButton(this, false);
        }

        //this will load the users as a list
        private async void LoadUsers()
        {
            _users = await _userRepository.GetAllUsersExceptAsync(_currentUserId);
            UserListView.ItemsSource = _users;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = UserSearchBar.Text;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                UserListView.ItemsSource = _users;
            }
            else
            {
                UserListView.ItemsSource = _users.Where(u => u.Username.ToLower().Contains(searchText.ToLower()));
            }
        }

        //userselects to message
        private async void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedUser = e.SelectedItem as User;

                // Navigate to the conversation page
                await Navigation.PushAsync(new MessageApp(_currentUserId, selectedUser));

            }

            // Deselect the selected user in the list
            UserListView.SelectedItem = null;
        }

        //Logouts the User
        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            // Clear the navigation stack and set LoginPage as the new root
            var loginPage = new LoginPage();
            Navigation.InsertPageBefore(loginPage, this);
            await Navigation.PopAsync();
        }

        //Navigates to the about us page
        private async void OnAboutUsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutUs());
        }

        private async void OnSwipeRight(object sender, SwipedEventArgs e)
        {
            bool answer = await DisplayAlert("Confirm", "You are about to navigate to the About Us page. Wanna see something cool", "Yes", "No");
            if (answer)
            {
                //  this will rotate the User List text slightly 
                await UserListView.FindByName<Label>("UserListLabel").RotateTo(-50, 100);

                // Navigate to the AboutUs page
                await Navigation.PushAsync(new AboutUs());
            }
            else
            {
                // this will rotate the User List text by 90 degrees
                await UserListView.FindByName<Label>("UserListLabel").RotateTo(50, 500);
            }
        }
    }
}

