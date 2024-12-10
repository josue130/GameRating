using GameRaitingAPI.DTOs;
using GameRaitingAPI.Filter;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GameRaitingAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static RouteGroupBuilder MapUsers(this RouteGroupBuilder group)
        {
            group.MapPost("/register", Register).AddEndpointFilter<ValidationFilter<RegisterRequestDTO>>();

            group.MapPost("/login", Login);
               
            return group;
        }

        static async Task<Results<NoContent, BadRequest<IEnumerable<IdentityError>>>> 
            Register(RegisterRequestDTO registerRequestDTO,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var user = new IdentityUser
            {
                UserName = registerRequestDTO.Email,
                Email = registerRequestDTO.Email
            };

            var result = await userManager.CreateAsync(user, registerRequestDTO.Password);

            if (result.Succeeded)
            {
                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.BadRequest(result.Errors);
            }
        }
        static async Task<Results<Ok<LoginResponseDTO>, BadRequest<string>>> Login(
            LoginRequestDTO loginRequestDTO, [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Email);

            if (user is null)
            {
                return TypedResults.BadRequest("incorrect Email or password");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user,
                loginRequestDTO.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var response = new LoginResponseDTO
                {
                    Token = "",
                    Expiration = DateTime.Now
                };
                return TypedResults.Ok(response);
            }
            else
            {
                return TypedResults.BadRequest("incorrect Email or password");
            }
        }
        
    }
}
