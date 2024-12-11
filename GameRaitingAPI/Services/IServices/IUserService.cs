using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GameRaitingAPI.Services.IServices
{
    public interface IUserService
    {
        Task<IdentityUser?> GetUser();
    }
}
