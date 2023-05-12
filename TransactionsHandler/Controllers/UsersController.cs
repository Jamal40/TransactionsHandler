using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TransactionsHandler.DTOs;

namespace TransactionsHandler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;

    public UsersController(IConfiguration configuration,
        UserManager<IdentityUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto credentials)
    {
        IdentityUser? user = await _userManager.FindByNameAsync(credentials.Username);
        if (user == null)
        {
            return BadRequest();
        }
        bool isPassCorrect = await _userManager.CheckPasswordAsync(user, 
            credentials.Password);
        if (!isPassCorrect)
        {
            return BadRequest();
        }

        var claimsList = await _userManager.GetClaimsAsync(user);

        string key = _configuration.GetValue<string>("SecretKey") ?? string.Empty;
        var keyInBytes = Encoding.ASCII.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(keyInBytes);
        var expire = DateTime.Now.AddMinutes(20);

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            claims: claimsList,
            signingCredentials: signingCredentials,
            expires: expire);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(jwt);
        return new TokenDto { Token = tokenString, Expiry = expire };
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var newEmployee = new IdentityUser
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
        };
        var creationResult = await _userManager.CreateAsync(newEmployee, registerDto.Password);

        if (!creationResult.Succeeded)
        {
            return BadRequest(creationResult.Errors);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, newEmployee.Id),
        };

        await _userManager.AddClaimsAsync(newEmployee, claims);

        return StatusCode(StatusCodes.Status201Created);
    }

}
