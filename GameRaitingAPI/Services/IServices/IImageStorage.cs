namespace GameRaitingAPI.Services.IServices
{
    public interface IImageStorage
    {
        Task Delete(string? route, string container);
        Task<string> Store(string container, IFormFile file);
        async Task<string> Update(string? route, string container, IFormFile file)
        {
            await Delete(route, container);
            return await Store(container, file);
        }
    }
}
