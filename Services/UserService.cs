using JwtAuthDemo.Models;

namespace JwtAuthDemo.Services;

public class UserService
{
    private static readonly List<User> Users = new()
    {
        new User{ ID = 1, Email = "test@email.com", Password = "password123"}
    };

    public User? Authenticate(string email, string password)
    {
        return Users.FirstOrDefault(u => u.Email == email && u.Password == password);
    }
}