using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Core.Entities;

namespace CodeExercise.Application.EntityMapper
{
    public static class ProblemTagMapper
    {
        public static ProblemTagDTO ToDTO(this ProblemTag problemTag)
        {
            if (problemTag == null) return null!;

            return new ProblemTagDTO
            {
                Id = problemTag.Id,
                ProblemId = problemTag.ProblemId,
                TagId = problemTag.TagId,
                CreatedAt = problemTag.CreatedAt,
                UpdatedAt = problemTag.UpdatedAt,
                IsDeleted = problemTag.IsDeleted,
                CreatedById = problemTag.CreatedById,
                UpdatedById = problemTag.UpdatedById,
                DeletedById = problemTag.DeletedById
            };
        }

        public static ProblemTag ToEntity(this ProblemTagDTO dto)
        {
            if (dto == null) return null!;

            return new ProblemTag
            {
                Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                ProblemId = dto.ProblemId,
                TagId = dto.TagId,
                CreatedById = dto.CreatedById,
                UpdatedAt = dto.UpdatedAt,
                UpdatedById = dto.UpdatedById,
                DeletedById = dto.DeletedById,
                IsDeleted = dto.IsDeleted,
                CreatedAt = dto.CreatedAt
            };
        }
    }
}
