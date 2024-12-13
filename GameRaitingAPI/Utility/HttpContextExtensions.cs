using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public static T GetValue<T>(this HttpContext context, string fieldName,
            T initialValue)
            where T : IParsable<T>
        {
            var valor = context.Request.Query[fieldName];

            if (valor.IsNullOrEmpty())
            {
                return initialValue;
            }

            return T.Parse(valor!, null);
        }
    }
}
