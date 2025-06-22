using CodeExercise.Core.Enums;

namespace CodeExercise.Application.DTO.Persistence
{
    public class ProblemDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DifficultyEnum Difficulty { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public string Slug { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Guid? CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }
        public Guid? DeletedById { get; set; }

        //public List<ProblemTagDTO> ProblemTags { get; set; }
        public List<TestCaseDTO> TestCases { get; set; }
        public List<TemplateCodeDTO> TemplateCodes { get; set; }
    }
}
