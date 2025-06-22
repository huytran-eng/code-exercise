using CodeExercise.Core.Enums;

namespace CodeExercise.Application.DTO.Request
{
    public class AdminUpdateProblemInDTO
    {
        public Guid Id { get;set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DifficultyEnum Difficulty { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public List<AdminUpdateProblemTestCaseInDTO> TestCases { get; set; }
        public List<AdminUpdateProblemTemplateCodeInDTO> TemplateCodes { get; set; }
    }

    public class AdminUpdateProblemTestCaseInDTO
    {
        public Guid? Id { get; set; }
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public string InputDisplay { get; set; }
        public string ExpectedOutputDisplay { get; set; }
        public bool IsHidden { get; set; } = false;
        public ActionFlagEnum ActionFlag { get; set; }
    }

    public class AdminUpdateProblemTemplateCodeInDTO
    {
        public Guid? Id { get; set; }
        public string UserTemplateCode { get; set; }
        public string HiddenTemplateCode { get; set; }

        public Guid ProgrammingLanguageId { get; set; }
        public ActionFlagEnum ActionFlag { get; set; }
    }

}
