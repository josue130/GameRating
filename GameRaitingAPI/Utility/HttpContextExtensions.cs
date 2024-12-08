using Microsoft.EntityFrameworkCore;

namespace GameRaitingAPI.Utility
{
    public static class HttpContextExtensions
    {
        public async static Task InsertCountGamesHeader<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if (httpContext is null) { throw new ArgumentNullException(nameof(httpContext)); }

            double count = await queryable.CountAsync();
            httpContext.Response.Headers.Append("games", count.ToString());
        }
    }
}
