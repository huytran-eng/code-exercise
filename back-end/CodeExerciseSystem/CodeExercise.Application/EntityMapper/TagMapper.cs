using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Core.Entities;

namespace CodeExercise.Application.EntityMapper
{
    public static class TagMapper
    {
        public static TagDTO ToDTO(this Tag tag)
        {
            if (tag == null) return null!; // Handle null entity

            return new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name,
                DisplayName = tag.DisplayName,
                Description = tag.Description,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt,
                IsDeleted = tag.IsDeleted,
                CreatedById = tag.CreatedById,
                UpdatedById = tag.UpdatedById,
                DeletedById = tag.DeletedById
            };
        }

        public static Tag ToEntity(this TagDTO dto)
        {
            if (dto == null) return null!; // Handle null DTO

            return new Tag
            {
                Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                Name = dto.Name,
                DisplayName = dto.DisplayName,
                Description = dto.Description,
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
