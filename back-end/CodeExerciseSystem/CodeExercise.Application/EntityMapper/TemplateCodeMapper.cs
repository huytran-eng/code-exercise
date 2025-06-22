using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Core.Entities;

namespace CodeExercise.Application.EntityMapper
{
    public static class TemplateCodeMapper
    {
        public static TemplateCodeDTO ToDTO(this TemplateCode templateCode)
        {
            if (templateCode == null) return null!; // Handle null entity

            return new TemplateCodeDTO
            {
                Id = templateCode.Id,
                ProgrammingLanguageId = templateCode.ProgrammingLanguageId,
                UserTemplateCode = templateCode.UserTemplateCode,
                HiddenTemplateCode = templateCode.HiddenTemplateCode,
                ProblemId = templateCode.ProblemId,
                CreatedAt = templateCode.CreatedAt,
                UpdatedAt = templateCode.UpdatedAt,
                IsDeleted = templateCode.IsDeleted,
                CreatedById = templateCode.CreatedById,
                UpdatedById = templateCode.UpdatedById,
                DeletedById = templateCode.DeletedById
            };
        }

        public static TemplateCode ToEntity(this TemplateCodeDTO dto)
        {
            if (dto == null) return null!; // Handle null DTO

            return new TemplateCode
            {
                Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                UserTemplateCode = dto.UserTemplateCode,
                HiddenTemplateCode = dto.HiddenTemplateCode,
                ProgrammingLanguageId = dto.ProgrammingLanguageId,
                ProblemId = dto.ProblemId,
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
