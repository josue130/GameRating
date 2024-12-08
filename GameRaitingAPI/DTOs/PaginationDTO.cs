namespace GameRaitingAPI.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int RecordsPerPage = 10;
        private readonly int MaxRecordsPerPage = 50;

        public int GetRecordsPerPage
        {
            get
            {
                return RecordsPerPage;
            }
            set
            {
                RecordsPerPage = (value > MaxRecordsPerPage) ? MaxRecordsPerPage : value;
            }
        }
    }
}
