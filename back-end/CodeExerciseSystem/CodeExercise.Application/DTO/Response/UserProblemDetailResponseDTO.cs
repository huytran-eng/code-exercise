using CodeExercise.Core.Enums;

namespace CodeExercise.Application.DTO.Response
{
    public class UserProblemDetailResponseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DifficultyEnum Difficulty { get; set; }
        public List<UserProblemDetailTestCasesDTO> TestCases { get; set; }
        public List<UserProblemDetailTemplateCodeDTO> TemplateCodes { get; set; }


    }

    public class UserProblemDetailTestCasesDTO
    {
        public Guid Id { get; set; }
        public string InputDisplay { get; set; }
        public string ExpectedOutputDisplay { get; set; }
    }

    public class UserProblemDetailTemplateCodeDTO
    {
        public Guid Id { get; set; }
        public string UserTemplateCode { get; set; }
        public Guid ProgrammingLanguageId { get; set; }
        public string ProgrammingLanguageDisplayName { get; set; }
        public string ProgrammingLanguageName { get; set; }

    }
}
