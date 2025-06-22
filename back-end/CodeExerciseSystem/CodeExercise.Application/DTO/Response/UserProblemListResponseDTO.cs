using CodeExercise.Core.Enums;

namespace CodeExercise.Application.DTO.Response
{
    public class UserProblemListResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DifficultyEnum Difficulty { get; set; }
        public string Slug { get; set; }
        public int SubmissionStatus { get; set; }
    }
}
