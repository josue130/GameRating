using GameRatingAPI.DTOs;
using GameRatingAPI.Filter;
using GameRatingAPI.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GameRatingAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static RouteGroupBuilder MapUsers(this RouteGroupBuilder group)
        {
            group.MapPost("/register", Register).AddEndpointFilter<ValidationFilter<RegisterRequestDTO>>();

            group.MapPost("/login", Login).AddEndpointFilter<ValidationFilter<LoginRequestDTO>>();

            group.MapPost("/new_admin", AddAdmin);

            group.MapPost("/delete_admin", DeleteAdmin).RequireAuthorization("admin");



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
                var response = await GenerateToken(loginRequestDTO, configuration, userManager);


                return TypedResults.Ok(response);
            }
            else
            {
                return TypedResults.BadRequest("incorrect Email or password");
            }
        }

        static async Task<Results<NoContent, NotFound>> AddAdmin(UpdateClaimsDTO updateClaimsDTO,
            [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(updateClaimsDTO.Email);
            if (user is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.AddClaimAsync(user, new Claim("admin", "true"));
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> DeleteAdmin(UpdateClaimsDTO updateClaimsDTO,
           [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(updateClaimsDTO.Email);
            if (user is null)
            {
                return TypedResults.NotFound();
            }

            await userManager.RemoveClaimAsync(user, new Claim("admin", "true"));
            return TypedResults.NoContent();
        }

        private async static Task<LoginResponseDTO>
            GenerateToken(LoginRequestDTO loginRequestDTO,
            IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var claims = new List<Claim>
            {
                new Claim("email", loginRequestDTO.Email),
                //more claims
            };

            var usuario = await userManager.FindByNameAsync(loginRequestDTO.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario!);

            claims.AddRange(claimsDB);

            var key = Keys.GetSecret(configuration);
            var creds = new SigningCredentials(key.First(), SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(2);

            var tokenDeSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new LoginResponseDTO
            {
                Token = token,
                Expiration = expiracion
            };
        }
    }
}
