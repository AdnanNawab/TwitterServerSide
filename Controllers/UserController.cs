
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Twitter.Repositories;
using Twitter.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using User.Models;
using Twitter.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;

namespace Twitter.Controllers;

[ApiController]

[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IUserRepository _user;

    private readonly IConfiguration _config;

    public UserController(ILogger<UserController> logger,
    IUserRepository user, IConfiguration config)
    {
        _logger = logger;
        _user = user;
        _config = config;

    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == UserConstants.Id).First().Value);

    }

    [HttpPost("SignUp")]
    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO Data)
    {

        var toCreateUser = new Users
        {
            Name = Data.Name.Trim(),
            Email = Data.Email.Trim().ToLower(),
            Password = BCrypt.Net.BCrypt.HashPassword(Data.Password)



        };

        var createdUser = await _user.Create(toCreateUser);

        return StatusCode(StatusCodes.Status201Created, createdUser.asDto);
    }


    [HttpPut("id")]
    [Authorize]
    public async Task<ActionResult> UpdateUser([FromRoute] long id,
    [FromBody] UserUpdateDTO Data)
    {
        var Id = GetUserIdFromClaims(User.Claims);

        var existingItem = await _user.GetById(Id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.Id != Id)
            return StatusCode(403, "You cannot update other's TODO");

        var toUpdateItem = existingItem with
        {
            Name = Data.Name is null ? existingItem.Name : Data.Name.Trim(),
            // IsComplete = !Data.IsComplete.HasValue ? existingItem.IsComplete : Data.IsComplete.Value,
        };

        await _user.Update(toUpdateItem);

        return NoContent();
    }





    [HttpPost("login")]


    public async Task<ActionResult<UserLoginResDTO>> Login([FromBody] UserLoginDTO Data)
    {
        var existingUser = await _user.GetByEmail(Data.Email);

        if (existingUser is null)
            return NotFound();

        if (!BCrypt.Net.BCrypt.Verify(Data.Password, existingUser.Password))


            // if (existingUser.Password != Data.Password)
            return BadRequest("Incorrect Password");


        var token = Generate(existingUser);

        var res = new UserLoginResDTO
        {
            Id = existingUser.Id,
            Email = existingUser.Email,
            Token = token,
        };
        return Ok(res);

    }

    private string Generate(Users user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(UserConstants.Id, user.Id.ToString()),

            new Claim(UserConstants.Email, user.Email),

        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



}