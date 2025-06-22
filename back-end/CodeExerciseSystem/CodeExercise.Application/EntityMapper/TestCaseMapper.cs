using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Core.Entities;

namespace CodeExercise.Application.EntityMapper
{
    public static class TestCaseMapper
    {
        public static TestCaseDTO ToDTO(this TestCase testCase)
        {
            return new TestCaseDTO
            {
                Id = testCase.Id,
                Input = testCase.Input,
                ExpectedOutput = testCase.ExpectedOutput,
                InputDisplay = testCase.InputDisplay,
                ProblemId = testCase.ProblemId,
                ExpectedOutputDisplay = testCase.ExpectedOutputDisplay,
                IsHidden = testCase.IsHidden,
                CreatedAt = testCase.CreatedAt,
                UpdatedAt = testCase.UpdatedAt,
                IsDeleted = testCase.IsDeleted,
                CreatedById = testCase.CreatedById,
                UpdatedById = testCase.UpdatedById,
                DeletedById = testCase.DeletedById
            };
        }

        public static TestCase ToEntity(this TestCaseDTO dto)
        {
            if (dto == null) return null!; // Handle null DTO

            return new TestCase
            {
                Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                Input = dto.Input,
                ExpectedOutput = dto.ExpectedOutput,
                InputDisplay = dto.InputDisplay,
                ExpectedOutputDisplay = dto.ExpectedOutputDisplay,
                ProblemId = dto.ProblemId,
                IsHidden = dto.IsHidden,
                CreatedAt = dto.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = dto.UpdatedAt ?? DateTime.UtcNow,
                IsDeleted = dto.IsDeleted,
                CreatedById = dto.CreatedById,
                UpdatedById = dto.UpdatedById,
                DeletedById = dto.DeletedById
            };
        }
    }
}
