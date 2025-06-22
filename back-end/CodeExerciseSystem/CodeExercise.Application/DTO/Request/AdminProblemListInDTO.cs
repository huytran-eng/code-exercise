using CodeExercise.Application.DTO.Common;

namespace CodeExercise.Application.DTO.Request
{
    public class AdminProblemListInDTO :BasePagingInDTO
    {
        public string? Title { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
