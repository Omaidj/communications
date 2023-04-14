using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

public class DatabaseService
{
    private SQLiteAsyncConnection _connection;

    public DatabaseService()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ComApp.db3");
        _connection = new SQLiteAsyncConnection(path);
    }

    public SQLiteAsyncConnection GetConnection()
    {
        return _connection;
    }
}

