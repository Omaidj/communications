using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;


namespace communications
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageApp : ContentPage
    {
        private MessageRepository _messageRepository;
        private int _currentUserId;
        private int _selectedUserId;
        private User _selectedUser;

        public MessageApp(int currentUserId, User selectedUser)
        {
            InitializeComponent();
            var databaseService = new DatabaseService();
            _messageRepository = new MessageRepository(databaseService.GetConnection());
            _currentUserId = currentUserId;
            _selectedUserId = selectedUser.Id;
            _selectedUser = selectedUser;
            LoadConversation();

            // Register the ItemSelected event handler for the ListView
            MessageListView.ItemSelected += OnMessageSelected;
        }

        private async void OnAboutButtonClicked(object sender, EventArgs e)
        {
            var aboutMessage = $"Username: {_selectedUser.Username}\nEmail: {_selectedUser.Email}";
            await DisplayAlert("About the User :)", aboutMessage, "Close");
        }

        private async void LoadConversation()
        {
            var messages = await _messageRepository.GetConversationAsync(_currentUserId, _selectedUserId);
            MessageListView.ItemsSource = messages;
        }

        private async void OnSendMessageClicked(object sender, EventArgs e)
        {
            var messageText = MessageEntry.Text;

            if (!string.IsNullOrWhiteSpace(messageText))
            {
                var message = new Message
                {
                    SenderId = _currentUserId,
                    ReceiverId = _selectedUserId,
                    Text = messageText,
                    Timestamp = DateTime.Now
                };

                await _messageRepository.SendMessageAsync(message);

                // Clear the message entry field
                MessageEntry.Text = string.Empty;

                // A notification that the message has been sent
                DependencyService.Get<INotificationService>().ShowNotification("Message Sent", "Your message has been sent.");

                // Refresh the conversation
                LoadConversation();
            }
        }


        private async void OnMessageSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedMessage = e.SelectedItem as Message;
            if (selectedMessage == null)
                return;

            // Deselect the message
            MessageListView.SelectedItem = null;

            await Navigation.PushAsync(new MessageDetailsPage(selectedMessage, _messageRepository, _currentUserId));
        }

        private async void OnCameraButtonClicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            var imageStream = await photo.OpenReadAsync();
            var imageData = new byte[imageStream.Length];
            await imageStream.ReadAsync(imageData, 0, imageData.Length);

            var message = new Message
            {
                SenderId = _currentUserId,
                ReceiverId = _selectedUserId,
                ImageData = imageData,
                Timestamp = DateTime.Now
            };

            await _messageRepository.SendMessageAsync(message);

            // A notification that the message has been sent
            DependencyService.Get<INotificationService>().ShowNotification("Message Sent", "Your message has been sent.");

            // Refresh the conversation
            LoadConversation();
        }



    }

}
