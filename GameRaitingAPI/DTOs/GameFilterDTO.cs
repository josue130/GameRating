using GameRaitingAPI.Utility;

namespace GameRaitingAPI.DTOs
{
    public class GameFilterDTO
    {
        public int Page { get; set; }
        public int RecordPerPage { get; set; }
        public PaginationDTO paginationDTO
        {
            get
            {
                return new PaginationDTO()
                {
                    Page = Page,
                    RecordsPerPage = RecordPerPage
                };
            }
        }

        public string? Name { get; set; }
        public int GenreId { get; set; }
        public string? Field { get; set; }
        public bool UpcomingGames { get; set; } = false;
        public bool OrderByAscending { get; set; } = true;

        public static ValueTask<GameFilterDTO> BindAsync(HttpContext context)
        {
            var page = context.GetValue(nameof(Page), 1);
            var recordsPerPage = context.GetValue(nameof(RecordPerPage), 10);
            var genreId = context.GetValue(nameof(GenreId), 0);

            var name = context.GetValue(nameof(Name), string.Empty);
            var field = context.GetValue(nameof(Field), string.Empty);
            var orderByAscending = context.GetValue(nameof(OrderByAscending), true);
            var upcomingGames = context.GetValue(nameof(UpcomingGames), false);
            var result = new GameFilterDTO
            {
                Page = page,
                RecordPerPage = recordsPerPage,
                Name = name,
                GenreId = genreId,
                Field = field,
                OrderByAscending = orderByAscending,
                UpcomingGames = upcomingGames
            };

            return ValueTask.FromResult(result);
        }
    }
}
