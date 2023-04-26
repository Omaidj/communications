using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using SQLite;
using Moq;


namespace MessageAppTest
{
    [TestClass]
    public class MessageRepositoryTests
    {
        [TestMethod]
        public async Task SendMessageAsync_ShouldAddNewMessage()
        {
            // Arrange
            var mockConnection = new Mock<SQLiteAsyncConnection>(MockBehavior.Strict);
            var messageRepository = new MessageRepository(mockConnection.Object);
            var message = new Message { SenderId = 1, ReceiverId = 2, Text = "Hello, world!", Timestamp = DateTime.Now };

            mockConnection.Setup(c => c.InsertAsync(message)).ReturnsAsync(1);

            // Act
            var result = await messageRepository.SendMessageAsync(message);

            // Assert
            Assert.IsTrue(result);
            mockConnection.Verify(c => c.InsertAsync(message), Times.Once);
        }
    }

}
