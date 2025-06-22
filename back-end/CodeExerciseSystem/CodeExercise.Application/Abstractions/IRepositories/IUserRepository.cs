using CodeExercise.Application.DTO.Persistence;

namespace CodeExercise.Application.Abstractions.IRepositories
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<UserDTO> GetAdminByUsernameAsync(string username);

    }
}
