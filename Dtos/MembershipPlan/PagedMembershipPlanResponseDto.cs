namespace GymSystem.Dtos.MembershipPlan
{
    public class PagedMembershipPlanResponseDto
    {
        public List<MembershipPlanResponseDto> Data { get; set; }
           = new List<MembershipPlanResponseDto>();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}
