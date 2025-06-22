using CodeExercise.Core.Enums;

namespace CodeExercise.Application.DTO.Response
{
    public class AdminProblemListResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DifficultyEnum Difficulty { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public string CreatedByName { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedByName { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
