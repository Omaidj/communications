using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace communications
{
    public partial class MessageDetailsPage : ContentPage
    {
        private readonly MessageRepository _messageRepository;
        private readonly int _currentUserId;

        public MessageDetailsPage(Message message, MessageRepository messageRepository, int currentUserId)
        {
            InitializeComponent();

            BindingContext = message;
            _messageRepository = messageRepository;
            _currentUserId = currentUserId;
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var selectedMessage = BindingContext as Message;
            if (selectedMessage == null)
                return;

            if (selectedMessage.SenderId != _currentUserId)
            {
                await DisplayAlert("Error", "You can only delete your own messages.", "OK");
                return;
            }

            var result = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this message?", "Delete", "Cancel");
            if (result)
            {
                await _messageRepository.DeleteMessageAsync(selectedMessage);
                await Navigation.PopAsync();
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            var selectedMessage = BindingContext as Message;
            if (selectedMessage == null)
                return;

            if (selectedMessage.SenderId != _currentUserId)
            {
                await DisplayAlert("Error", "You can only edit your own messages.", "OK");
                return;
            }

            string newText = await DisplayPromptAsync("Update Message", "Enter the new message text:", "OK", "Cancel", null, -1, Keyboard.Default, selectedMessage.Text);
            if (string.IsNullOrEmpty(newText))
                return;

            selectedMessage.Text = newText;
            bool result = await _messageRepository.UpdateMessageAsync(selectedMessage);
            if (result)
            {
                await DisplayAlert("Success", "Message updated successfully.", "OK");
            }
        }
        private async void OnShareClicked(object sender, EventArgs e)
        {
            var selectedMessage = BindingContext as Message;
            if (selectedMessage == null)
                return;

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = selectedMessage.Text,
                Title = "Share Message"
            });
        }
    }
}
