using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service.Dto;
using BCrypt.Net;
using Service.Interface;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IService<UserDto> _userService;
    private readonly IConfiguration _config;

    public AuthController(IService<UserDto> userService, IConfiguration config)
    {
        this._userService = userService;
        _config = config;

    }
    [HttpPost("register")]
    
    public async Task<ActionResult<object>> Register(UserDto user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var newUser = await _userService.AddItem(user);
        string token = GenerateToken(newUser);
        newUser.Password = null;
        return Ok(new { token, user = newUser });  // ✅ מחזיר token + user
    }
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto user)
    {
        List<UserDto> users = await _userService.GetAll();
        UserDto existingUser = users.FirstOrDefault(u => u.Email == user.Email);
        if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            return Unauthorized("The email or password is incorrect");
        string token = GenerateToken(existingUser);
        existingUser.Password = null;
        return Ok(new { token, user = existingUser });
    }
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idClaim == null) return Unauthorized();
        
        int userId = int.Parse(idClaim);
        var user = await _userService.GetById(userId);
        if (user == null) return NotFound();
        
        user.Password = null;
        return Ok(user);
    }
    private string GenerateToken(UserDto user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.NameUser),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}