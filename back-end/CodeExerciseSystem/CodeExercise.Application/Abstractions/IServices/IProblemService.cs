using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Common;

namespace CodeExercise.Application.Abstractions.IServices
{
    public interface IProblemService
    {
        Task<ResponseResult<ProblemDTO>> AdminGetProblemDetailAsync(Guid problemId);
        Task<ResponseResult<Guid>> AdminCreateProblemAsync(AdminCreateProblemInDTO problem, Guid userId);
        Task<ResponseResult<bool>> AdminUpdateProblemAsync(AdminUpdateProblemInDTO problem, Guid userId);
        Task<ResponseResult<List<AdminProblemListResponseDTO>>> AdminGetProblemsAsync(AdminProblemListInDTO adminProblemListInDTO);
        Task<ResponseResult<bool>> AdminDeleteProblemAsync(Guid problemId, Guid userId);


        Task<ResponseResult<List<UserProblemListResponseDTO>>> UserGetProblemsAsync(UserProblemListInDTO filter, Guid? userId);
        Task<ResponseResult<UserProblemDetailResponseDTO>> UserGetProblemDetailAsync(string slug);

    }
}
