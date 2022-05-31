using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pi_Books.Data;
using Pi_Books.Data.Models;
using Pi_Books.Data.ViewModels.Authentication;

namespace Pi_Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;

        //Refresh Tokens
        private readonly TokenValidationParameters tokenValidationParameters;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
            this.configuration = configuration;
            this.tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("new-user-sign-up")]
        public async Task<IActionResult> Register([FromBody] RegisterVM payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("All input fields are required!");
            }
            var userExist = await this.userManager.FindByEmailAsync(payload.Email);

            if (userExist != null)
            {
                return BadRequest($"User {payload.Email} already exists in the system!");
            }
            ApplicationUser newUser = new ApplicationUser()
            {
                Email = payload.Email,
                UserName = payload.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await this.userManager.CreateAsync(newUser, payload.Password);
            if (!result.Succeeded)
            {
                return BadRequest("New User Sign-up Failed!");
            }

            switch (payload.Role)
            {
                case "Admin":
                    await userManager.AddToRoleAsync(newUser, UserRoles.Admin);
                    break;

                case "Publisher":
                    await userManager.AddToRoleAsync(newUser, UserRoles.Publisher);
                    break;

                case "Author":
                    await userManager.AddToRoleAsync(newUser, UserRoles.Author);
                    break;

                default:
                    await userManager.AddToRoleAsync(newUser, UserRoles.User);
                    break;
            }

            return Created(nameof(Register), $"New User Sign-up is Successful with {payload.Email}!");
        }

        [HttpPost("user-sign-in")]
        public async Task<IActionResult> Login([FromBody] LoginVM payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("All input fields are required!");
            }

            var user = await userManager.FindByEmailAsync(payload.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, payload.Password))
            {
                var tokenValue = await GenerateJwtTokenAsync(user, "");
                return Ok(tokenValue);
            }

            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public async ValueTask<IActionResult> RefreshToken([FromBody] TokenRequestVM payload)
        {
            try
            {
                var result = await VerifyAndGenerateTokenAsync(payload);
                if (result == null)
                {
                    return BadRequest("Invalid Tokens!");
                }
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }

        private async Task<AuthResultVM> VerifyAndGenerateTokenAsync(TokenRequestVM payload)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                //Check#1 - JWT Token Format
                var tokenInVarification = jwtTokenHandler.ValidateToken(payload.Token, tokenValidationParameters, out var validatedToken);

                //Check#2 - Encryption Algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false) return null;
                }

                //Check#3 - Expire Date Validation (need advanced alternative)
                var utcTokenExpireDate = long.Parse(tokenInVarification
                    .Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = UnixTimeStamptoDateTimeUTC(utcTokenExpireDate);
                if (expireDate > DateTime.UtcNow) throw new Exception("Token has not expired yet!");

                //Check#4 - Refresh Token Already Exist in the Db
                var dbRefreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == payload.RefreshToken);
                if (dbRefreshToken == null) throw new Exception("Refresh Token not found in the Db!");
                else
                {
                    //Check#5 - Validate Id
                    var jwtTokenId = tokenInVarification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                    if (dbRefreshToken.JwtId != jwtTokenId) throw new Exception("Token doesn't match!");

                    //Check#6 Refresh Token Expiry
                    if (dbRefreshToken.DateExpire <= DateTime.UtcNow)
                        throw new Exception("Your Refresh Token has Expired Already! Please Re-authenticate!");

                    //Check#7 Refresh Token Revoked
                    if (dbRefreshToken.IsRevoked) throw new Exception("Refresh Token is Revoked!");

                    //Generate New Token (with existing Refresh Token)
                    var dbUserData = await userManager.FindByIdAsync(dbRefreshToken.UserId);
                    var newTokenResponse = GenerateJwtTokenAsync(dbUserData, payload.RefreshToken);

                    return await newTokenResponse;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                var dbRefreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == payload.RefreshToken);
                //Generate New Token (with existing Refresh Token)
                var dbUserData = await userManager.FindByIdAsync(dbRefreshToken.UserId);
                var newTokenResponse = GenerateJwtTokenAsync(dbUserData, payload.RefreshToken);

                return await newTokenResponse;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        private async Task<AuthResultVM> GenerateJwtTokenAsync(ApplicationUser user, string existingRefreshTOken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Add User Roles
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(this.configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: this.configuration["JWT:Issuer"],
                audience: this.configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(10),//5~10 Min Default
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = new RefreshToken();

            if (string.IsNullOrEmpty(existingRefreshTOken))
            {
                refreshToken = new RefreshToken()
                {
                    JwtId = token.Id,
                    IsRevoked = false,
                    UserId = user.Id,
                    DateAdded = DateTime.Now,
                    DateExpire = DateTime.Now.AddMonths(6),
                    Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
                };

                await this.context.RefreshTokens.AddAsync(refreshToken);
                await this.context.SaveChangesAsync();
            }
            var response = new AuthResultVM()
            {
                Token = jwtToken,
                RefreshToken = (string.IsNullOrEmpty(existingRefreshTOken)) ? refreshToken.Token : existingRefreshTOken,
                ExpiresAt = token.ValidTo
            };

            return response;
        }

        private DateTime UnixTimeStamptoDateTimeUTC(long unixTimeStamp)
        {
            var dateTimeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeValue = dateTimeValue.AddSeconds(unixTimeStamp);
            return dateTimeValue;
        }
    }
}