using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Entities;

namespace CodeExercise.Application.Abstractions.IRepositories
{
    public interface IProblemRepository
    {
        Task<(List<AdminProblemListResponseDTO> Problems, int TotalCount)> AdminGetProblemsAsync(AdminProblemListInDTO filter);
        Task<Guid> AdminCreateProblemAsync(Problem problem, List<TestCase> testCases, List<TemplateCode> templateCodes);

        Task<ProblemDTO> AdminGetProblemDetailByIdAsync(Guid problemId);
        Task<bool> AdminUpdateProblemAsync(Problem problem, List<TestCase> updateTestCases, List<TestCase> newTestCases, List<TemplateCode> updateTemplateCode, List<TemplateCode> newTemplateCode);
        Task<bool> AdminDeleteProblemAsync(Problem problem);
        Task<(List<UserProblemListResponseDTO> Problems, int TotalCount)> UserGetProblemsAsync(UserProblemListInDTO problemFilter, Guid? userId);
        Task<UserProblemDetailResponseDTO> UserGetProblemDetailBySlugAsync(string slug);
    }
}
