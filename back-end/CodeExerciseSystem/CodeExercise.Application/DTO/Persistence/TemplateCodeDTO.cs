namespace CodeExercise.Application.DTO.Persistence
{
    public class TemplateCodeDTO
    {
        public Guid Id { get; set; }
        public string UserTemplateCode { get; set; }
        public string HiddenTemplateCode { get; set; }

        public Guid ProblemId { get; set; }
        public Guid ProgrammingLanguageId { get; set; }
        public string ProgrammingLanguageName { get; set; }
        public string ProgrammingLanguageDisplayName { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }
        public Guid? DeletedById { get; set; }
    }
}
