using CodeExercise.Application.DTO.Common;

namespace CodeExercise.Application.DTO.Request
{
    public class AdminTagListInDTO : BasePagingInDTO
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
