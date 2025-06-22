using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Core.Entities;

namespace CodeExercise.Application.EntityMapper
{
    public static class ProblemMapper
    {
        public static ProblemDTO ToDTO(this Problem problem)
        {
            if (problem == null) return null!;

            return new ProblemDTO
            {
                Id = problem.Id,
                Title = problem.Title,
                Description = problem.Description,
                Difficulty = problem.Difficulty,
                TimeLimit = problem.TimeLimit,
                MemoryLimit = problem.MemoryLimit,
                Slug = problem.Slug,
                CreatedAt = problem.CreatedAt,
                UpdatedAt = problem.UpdatedAt,
                IsDeleted = problem.IsDeleted,
                CreatedById = problem.CreatedById,
                UpdatedById = problem.UpdatedById,
                DeletedById = problem.DeletedById
            };
        }

        public static Problem ToEntity(this ProblemDTO dto)
        {
            if (dto == null) return null!;

            return new Problem

            {
                Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Difficulty = dto.Difficulty,
                TimeLimit = dto.TimeLimit,
                MemoryLimit = dto.MemoryLimit,
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
