using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MessageRepository
{
    private readonly SQLiteAsyncConnection _connection;

    public MessageRepository(SQLiteAsyncConnection connection)
    {
        _connection = connection;
        _connection.CreateTableAsync<Message>().Wait();
    }

    public async Task<List<Message>> GetConversationAsync(int userId1, int userId2)
    {
        return await _connection.Table<Message>()
            .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) || (m.SenderId == userId2 && m.ReceiverId == userId1))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }
    //send messages
    public async Task SendMessageAsync(Message message)
    {
        await _connection.InsertAsync(message);
    }

    //delete messages
    public async Task DeleteMessageAsync(Message message)
    {
        await _connection.DeleteAsync(message);
    }

    public async Task<bool> UpdateMessageAsync(Message message)
    {
        var existingMessage = await _connection.Table<Message>()
            .FirstOrDefaultAsync(m => m.Id == message.Id);

        if (existingMessage == null)
            return false;

        existingMessage.Text = message.Text;
        existingMessage.Timestamp = message.Timestamp;

        int rowsUpdated = await _connection.UpdateAsync(existingMessage);

        return rowsUpdated > 0;
    }


}

