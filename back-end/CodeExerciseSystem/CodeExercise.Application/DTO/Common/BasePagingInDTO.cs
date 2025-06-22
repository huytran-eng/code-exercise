namespace CodeExercise.Application.DTO.Common
{
    public class BasePagingInDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public int? OrderBy { get; set; }
    }
}
