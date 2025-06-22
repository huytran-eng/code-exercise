using CodeExercise.Application.DTO.Common;

namespace CodeExercise.Application.DTO.Request
{
    public class UserProblemListInDTO: BasePagingInDTO
    {
        public string? Title { get; set; }
    }
}
