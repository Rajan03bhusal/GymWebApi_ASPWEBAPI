namespace GymSystem.Dtos.Trainer
{
    public class PagedTrainerResponseDto
    {
        public List<TrainerResponseDto> Data { get; set; }
           = new();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}
