using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserRepository
{
    private readonly SQLiteAsyncConnection _connection;

    public UserRepository(SQLiteAsyncConnection connection)
    {
        _connection = connection;
        _connection.CreateTableAsync<User>().Wait();
    }

    //registering the user in the database
    public async Task<bool> RegisterUserAsync(User user)
    {
        var existingUser = await _connection.Table<User>().Where(u => u.Username == user.Username || u.Email == user.Email).FirstOrDefaultAsync();

        if (existingUser == null)
        {
            await _connection.InsertAsync(user);
            return true;
        }

        return false;
    }

    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var user = await _connection.Table<User>().Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();

        return user;
    }
    //search bar, this searches all the users
    public async Task<List<User>> GetAllUsersExceptAsync(int currentUserId)
    {
        var users = await _connection.Table<User>().Where(u => u.Id != currentUserId).ToListAsync();
        return users;
    }

    //forgot password
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _connection.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
    }


    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _connection.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
    }

}
