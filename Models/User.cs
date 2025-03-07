namespace JwtAuthDemo.Models;

public class User
{
    public int ID { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}