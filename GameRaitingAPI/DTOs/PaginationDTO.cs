using GameRaitingAPI.Utility;
using Microsoft.IdentityModel.Tokens;

namespace GameRaitingAPI.DTOs
{
    public class PaginationDTO
    {
        private const int InitialPageValue = 1;
        private const int InitialRecordsPerPageValue= 10;
        public int Page { get; set; } = InitialPageValue;
        private int recordsPerPage = InitialRecordsPerPageValue;
        private readonly int MaxRecordsPerPage = 50;

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                recordsPerPage = (value > MaxRecordsPerPage) ? MaxRecordsPerPage : value;
            }
        }
        public static ValueTask<PaginationDTO> BindAsync(HttpContext context)
        {
            var page = context.GetValue(nameof(Page),InitialPageValue);
            var recordsPerPage = context.GetValue(nameof(RecordsPerPage), InitialRecordsPerPageValue);
            //var recordsPerPage = context.Request.Query[nameof(RecordsPerPage)];




            var result = new PaginationDTO
            {
                Page = page,
                RecordsPerPage = recordsPerPage
            };

            return ValueTask.FromResult(result);

        }

    }
}
