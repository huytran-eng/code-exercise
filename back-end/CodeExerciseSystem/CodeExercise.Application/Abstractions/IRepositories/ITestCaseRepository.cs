using CodeExercise.Core.Entities;

namespace CodeExercise.Application.Abstractions.IRepositories
{
    public interface ITestCaseRepository
    {
        Task<List<TestCase>> AdminGetTestCaseByProblemId(Guid problemId);
    }

}
