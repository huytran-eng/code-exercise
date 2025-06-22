using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Common;

namespace CodeExercise.Application.Abstractions.IServices
{
    public  interface IAdminService
    {
        Task<ResponseResult<AdminLoginResponseDTO>> AdminLoginAsync(AdminLoginInDTO loginRequest);
    }
}
