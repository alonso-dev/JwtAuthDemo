using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthDemo.Models;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthDemo.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        var existingUser = _userService.Authenticate(user.Email, user.Password);
        if(existingUser == null) return Unauthorized("Invalid credentials.");

        // Generate JWT Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("ThisIsASecretKeyForJwtAuth");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.ID.ToString()),
                new Claim(ClaimTypes.Email, existingUser.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString});
    }
}