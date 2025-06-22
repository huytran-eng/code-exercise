using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Common;

namespace CodeExercise.Application.Abstractions.IServices
{
    public interface IProgrammingLanguageService
    {
        Task<ResponseResult<List<AdminProgrammingLanguageListResponseDTO>>> AdminGetAllProgrammingLanguagesAsync();

    }
}
