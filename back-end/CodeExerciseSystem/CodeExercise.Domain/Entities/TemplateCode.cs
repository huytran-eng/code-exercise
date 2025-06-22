using CodeExercise.Core.Common;
using CodeExercise.Core.Utils;

namespace CodeExercise.Core.Entities
{
    public class TemplateCode : BaseModel
    {
        public Guid ProblemId { get; set; }
        public Guid ProgrammingLanguageId { get; set;  }
        public string UserTemplateCode { get; set; }
        public string HiddenTemplateCode { get; set; }


        public TemplateCode(string userTemplateCode, string hiddenTemplateCode, Guid programmingLanguageId ,Guid problemId, Guid createdById)
        {
            Validate(userTemplateCode, hiddenTemplateCode);

            Id = Guid.NewGuid();
            UserTemplateCode = userTemplateCode;
            HiddenTemplateCode = hiddenTemplateCode;
            ProblemId = problemId;
            ProgrammingLanguageId = programmingLanguageId;
            CreatedById = createdById;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string userTemplateCode, string hiddenTemplateCode, Guid updatedById)
        {
            Validate(userTemplateCode, hiddenTemplateCode);

            UserTemplateCode = userTemplateCode;
            HiddenTemplateCode = hiddenTemplateCode;
            UpdatedById = updatedById;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void Validate(string userTemplateCode, string hiddenTemplateCode)
        {
            if (string.IsNullOrWhiteSpace(userTemplateCode))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.USER_TEMPLATE_CODE_EMPTY_VALIDATE);

            if (string.IsNullOrWhiteSpace(hiddenTemplateCode))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.HIDDEN_TEMPLATE_CODE_EMPTY_VALIDATE);

        }

        public TemplateCode() { }
    }
}
