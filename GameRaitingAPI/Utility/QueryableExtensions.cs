using GameRaitingAPI.DTOs;

namespace GameRaitingAPI.Utility
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Pagination<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.GetRecordsPerPage)
                .Take(paginationDTO.GetRecordsPerPage);
        }
    }
}
