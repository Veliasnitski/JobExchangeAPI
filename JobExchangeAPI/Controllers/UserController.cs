using Data;
using Data.Models;
using JobExchangeAPI.Helpers;
using JobExchangeAPI.Models.RequestModels;
using JobExchangeAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace JobExchangeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JobExchangeDBContext _jobExchangeDBContext;
        public UserController(JobExchangeDBContext jobExchangeDBContext)
        {
            _jobExchangeDBContext = jobExchangeDBContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel user)
        {
            if (user == null)
                return BadRequest();

            //TODO: add hash to password
            try
            {
                var curUser = await _jobExchangeDBContext.Users
                    .FirstOrDefaultAsync(x => x.Username == user.Username);

                if (curUser == null)
                    return NotFound(new { Message = "User not Found!" });

                if (!PasswordHasher.VerifyPassword(user.Password, curUser.Password))
                    return BadRequest(new { Message = "Password is incorrect!" });

                curUser.Token = CreateJWT(curUser);

                return Ok(new { Token = curUser.Token, Message = "Login Success!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();
            if (await CheckUserNameExistAsync(user.Username))
                return BadRequest(new { Message = "Username already exist!" });
            if (!string.IsNullOrEmpty(user.Email) && await CheckEmailExistAsync(user.Email))
                return BadRequest(new { Message = "Email already exist!" });
            var pass = CheckPasswordExistAsync(user.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass });

            try
            {
                user.Password = PasswordHasher.HashPassword(user.Password);
                user.Role = UserRole.User;
                // TODO: token
                user.Token = "";
                await _jobExchangeDBContext.Users.AddAsync(user);
                await _jobExchangeDBContext.SaveChangesAsync();

                return Ok(new { Message = "User Registered!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        private Task<bool> CheckUserNameExistAsync(string username) => _jobExchangeDBContext.Users.AnyAsync(x => x.Username == username);
        private Task<bool> CheckEmailExistAsync(string email) => _jobExchangeDBContext.Users.AnyAsync(x => x.Email == email);
        private string CheckPasswordExistAsync(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < LengthFields.minPasswordLen)
                sb.Append(ErrorMessages.MinPasswordLen + Environment.NewLine);
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
                sb.Append(ErrorMessages.PasswordAlphanumeric + Environment.NewLine);
            if (!Regex.IsMatch(password, "[^A-Za-z0-9]"))
                sb.Append(ErrorMessages.PasswordSpecChars + Environment.NewLine);

            return sb.ToString();
        }

        private string CreateJWT(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Constants.TokenKey);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                 Subject = identity,
                 Expires = DateTime.UtcNow.AddHours(5),
                 SigningCredentials = credentials,
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
