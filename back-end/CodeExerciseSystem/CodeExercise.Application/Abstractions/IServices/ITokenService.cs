using CodeExercise.Application.DTO.Persistence;

namespace CodeExercise.Application.Abstractions.IServices
{
    public interface ITokenService
    {
        string CreateToken(UserDTO user);
    }
}
