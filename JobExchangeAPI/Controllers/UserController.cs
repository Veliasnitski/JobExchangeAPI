using Data;
using Data.Models;
using JobExchangeAPI.Helpers;
using JobExchangeAPI.Models.DTO;
using JobExchangeAPI.Models.RequestModels;
using JobExchangeAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

            try
            {
                var curUser = await _jobExchangeDBContext.Users
                    .FirstOrDefaultAsync(x => x.Username == user.Username);

                if (curUser == null)
                    return NotFound(new { Message = "User not Found!" });

                if (!PasswordHasher.VerifyPassword(user.Password, curUser.Password))
                    return BadRequest(new { Message = "Password is incorrect!" });

                curUser.Token = CreateJWT(curUser);
                var newAccessToken = curUser.Token;
                var newRefreshToken = CreateRefreshToken();
                curUser.RefreshToken = newRefreshToken;
                curUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1);
                await _jobExchangeDBContext.SaveChangesAsync();

                return Ok(new TokenApiDTO { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] SignUp signUp)
        {
            if (signUp == null)
                return BadRequest();

            var user = new User
            {
                Username = signUp.Username,
                Password = signUp.Password,
                FirstName= signUp.FirstName,
                LastName= signUp.LastName,
                Role = UserRole.User,
                Email = signUp.Email,
                Token = signUp.Token
            };
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
                user.Token = CreateJWT(user);
                var newAccessToken = user.Token;
                var newRefreshToken = CreateRefreshToken();
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(1);

                await _jobExchangeDBContext.SaveChangesAsync();
                await _jobExchangeDBContext.Users.AddAsync(user);
                await _jobExchangeDBContext.SaveChangesAsync();

                return Ok(new { Message = "User Registered!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        #region other

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
                new Claim(ClaimTypes.Name, user.Username),
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                 Subject = identity,
                 Expires = DateTime.UtcNow.AddHours(1),
                 SigningCredentials = credentials,
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);
            var tokenInUser = _jobExchangeDBContext.Users
                .Any(x => x.RefreshToken == refreshToken);

            if (tokenInUser)
            {
                return CreateRefreshToken();
            }

            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(Constants.TokenKey);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is invalid token!");

            return principal;
        }

        #endregion

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _jobExchangeDBContext.Users.ToListAsync());
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(TokenApiDTO tokenApiDTO)
        {
            if (tokenApiDTO is null)
                return BadRequest("Invalid client request");

            var accessToken = tokenApiDTO.AccessToken;
            var refreshToken = tokenApiDTO.RefreshToken;
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = await _jobExchangeDBContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return BadRequest("Invalid request!");

            var newAccessToken = CreateJWT(user);
            var newRefreshToken = CreateRefreshToken();
            
            user.RefreshToken = newRefreshToken;
            await _jobExchangeDBContext.SaveChangesAsync();

            return Ok(new TokenApiDTO { AccessToken = accessToken, RefreshToken = newRefreshToken });
        }
    }
}
