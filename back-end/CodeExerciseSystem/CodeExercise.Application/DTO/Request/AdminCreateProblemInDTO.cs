using CodeExercise.Core.Enums;

namespace CodeExercise.Application.DTO.Request
{
    public class AdminCreateProblemInDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DifficultyEnum Difficulty { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public List<AdminCreateProblemTestCaseInDTO> TestCases { get; set; }
        //public List<Guid> TagIds { get; set; }
        public List<AdminCreateProblemTemplateCodeInDTO> TemplateCodes { get; set; }
    }

    public class AdminCreateProblemTestCaseInDTO
    {
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public string InputDisplay { get; set; }
        public string ExpectedOutputDisplay { get; set; }
        public bool IsHidden { get; set; } = false;
    }

    public class AdminCreateProblemTemplateCodeInDTO
    {
        public string UserTemplateCode { get; set; }
        public string HiddenTemplateCode { get; set; }
        public Guid ProgrammingLanguageId { get; set; }
    }

}
