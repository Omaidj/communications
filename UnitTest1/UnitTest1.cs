using System.Data.SqlTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;
using System.Threading.Tasks;

namespace UnitTest1
{ 

    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public async Task TestRegisterUserAsync()
        {
            
            var connection = new SQLiteAsyncConnection(":memory:");
            var repository = new UserRepository(connection);
            var user = new User
            {
                Username = "testuser",
                Password = "password",
                Email = "testuser@example.com"
            };

            
            var result1 = await repository.RegisterUserAsync(user);
            var result2 = await repository.RegisterUserAsync(user);

           
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }
    }

   [TestClass]
   public class MessageRepositoryTest
   {
        [TestMethod]
        public async Task TestSendMessageAsync()
        {
        var connection = new SQLiteAsyncConnection(":memory:");
        var repository = new MessageRepository(connection);
        var message = new Message
        {
            SenderId = 1,
            ReceiverId = 2,
            Text = "Hello world!",
            Timestamp = DateTime.Now
        };

        await repository.SendMessageAsync(message);
        var messages = await repository.GetConversationAsync(1, 2);

        Assert.IsNotNull(messages);
        Assert.AreEqual(1, messages.Count);
        Assert.AreEqual("Hello world!", messages[0].Text);
        }

        //[TestMethod]
        //public async Task TestUpdateMessageAsync()
        //{
            
            //var connection = new SQLiteAsyncConnection(":memory:");
            //var repository = new MessageRepository(connection);
            //var message = new Message
            //{
            //    SenderId = 1,
            //    ReceiverId = 2,
            //    Text = "Hello world!",
            //    Timestamp = DateTime.Now
            //};
            //await repository.SendMessageAsync(message);
            //
            //
            //message.Text = "Updated message";
            //bool result = await repository.UpdateMessageAsync(message);
            //var updatedMessage = await repository.GetConversationAsync(1, 2);
            //
            //// Assert
            //Assert.IsTrue(result);
            //Assert.AreEqual(1, updatedMessage.Count);
            //Assert.AreEqual("Updated message", updatedMessage[0].Text);
        //}
    }

}