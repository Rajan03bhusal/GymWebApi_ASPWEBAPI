namespace GymSystem.Dtos
{
    public class PagedMemberResponseDto
    {
        public List<MemberResponseDto> Data { get; set; }
            = new List<MemberResponseDto>();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}
