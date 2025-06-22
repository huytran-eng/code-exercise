using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Core.Entities;

namespace CodeExercise.Application.Mapper
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(this User user)
        {
            if (user == null) return null!; // Handle null entity

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                PasswordHash = user.PasswordHash ?? Array.Empty<byte>(),
                PasswordSalt = user.PasswordSalt ?? Array.Empty<byte>(),
                BirthDate = user.BirthDate,
                Address = user.Address,
                Phone = user.Phone,
                Note = user.Note,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsDeleted = user.IsDeleted,
                CreatedById = user.CreatedById,
                UpdatedById = user.UpdatedById,
                DeletedById = user.DeletedById
            };
        }

        public static User ToEntity(this UserDTO dto)
        {
            if (dto == null) return null!; // Handle null DTO

            return new User
            {
                Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = dto.PasswordHash ?? Array.Empty<byte>(),
                PasswordSalt = dto.PasswordSalt ?? Array.Empty<byte>(),
                BirthDate = dto.BirthDate,
                Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address,
                Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone,
                Note = string.IsNullOrWhiteSpace(dto.Note) ? null : dto.Note,
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
