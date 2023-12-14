using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.DTOs;
using Facilitat.CLOUD.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;


[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(UserDTO user)
    {
        var existingUser = await _userService.GetUserByEmailAsync(user.Email);
        if (existingUser != null)
        {
            return BadRequest("User already exists.");
        }

        // Hash the password
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        // Create the user
        var result = await _userService.AddUserAsync(user);
        if (result)
        {
            return Ok(user); // Or return CreatedAtAction if you want to return the created user's details
        }

        return BadRequest("Unable to create user.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginData)
    {
        // Get the user by email
        var user = await _userService.GetUserByEmailAsync(loginData.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginData.Password, user.Password))
        {
            return Unauthorized("Invalid credentials.");
        }

        // Generate the JWT token for the user
        var token = GenerateJwtToken(user);

        return Ok(new { token });
    }

    private string GenerateJwtToken(UserDTO user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                // Add more claims as needed
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
