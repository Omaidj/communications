using SQLite;
using System;

public class Message
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Text { get; set; }
    public byte[] ImageData { get; set; }
    public DateTime Timestamp { get; set; }
}
