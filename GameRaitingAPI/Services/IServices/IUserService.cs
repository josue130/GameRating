using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GameRatingAPI.Services.IServices
{
    public interface IUserService
    {
        Task<IdentityUser?> GetUser();
    }
}
