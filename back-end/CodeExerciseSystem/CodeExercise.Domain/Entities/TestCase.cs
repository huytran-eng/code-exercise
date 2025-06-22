using CodeExercise.Core.Common;
using CodeExercise.Core.Utils;
using System.Xml.Linq;

namespace CodeExercise.Core.Entities
{
    public class TestCase : BaseModel
    {
        public string Input { get; set; } 
        public string ExpectedOutput { get; set; } 
        public string InputDisplay { get; set; }
        public string ExpectedOutputDisplay { get; set; }

        public bool IsHidden { get; set; } = false;

        public Guid ProblemId { get; set; }

        public TestCase(string input, string expectedOutput, string inputDisplay, string expectedOutputDisplay, Guid createdById, Guid problemId, bool isHidden = true)
        {
            Validate(input, expectedOutput,  inputDisplay, expectedOutputDisplay);
            Id = Guid.NewGuid();
            Input = input;
            ExpectedOutput = expectedOutput;
            InputDisplay = inputDisplay;
            ExpectedOutputDisplay = expectedOutputDisplay;
            IsHidden = isHidden;
            ProblemId = problemId;
            CreatedById = createdById;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string input, string expectedOutput, string inputDisplay, string expectedOutputDisplay, bool isHidden, Guid updatedById )
        {
            Validate(input, expectedOutput, inputDisplay, expectedOutputDisplay);
            Input = input;
            ExpectedOutput = expectedOutput;
            InputDisplay = inputDisplay;
            ExpectedOutputDisplay = expectedOutputDisplay;
            IsHidden = isHidden;
            UpdatedById = updatedById;                                                                                                                                              
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete(Guid deletedById)
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedById = deletedById;
        }
        private static void Validate(string input, string expectedOutput, string inputDisplay, string expectedOutputDisplay)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.TEST_CASE_INPUT_EMPTY_VALIDATE);
            if (string.IsNullOrWhiteSpace(expectedOutput))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.TEST_CASE_OUTPUT_EMPTY_VALIDATE);
            if (string.IsNullOrWhiteSpace(inputDisplay))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.TEST_CASE_INPUT_DISPLAY_EMPTY_VALIDATE);
            if (string.IsNullOrWhiteSpace(expectedOutputDisplay))
                throw new ArgumentWithMessageCodeException(MessageCodeEnum.TEST_CASE_OUTPUT_DISPLAY_EMPTY_VALIDATE);
        }

        public TestCase() { }
    }
}
