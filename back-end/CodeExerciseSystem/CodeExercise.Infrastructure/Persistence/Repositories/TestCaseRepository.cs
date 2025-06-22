using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Core.Entities;
using Dapper;

namespace CodeExercise.Infrastructure.Persistence.Repositories
{
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly DapperContext _context;
        public TestCaseRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<TestCase>> AdminGetTestCaseByProblemId(Guid problemId)
        {
            var sqlQuery = @"SELECT * FROM TestCase tc WHERE tc.ProblemId = @ProblemId AND tc.IsDeleted = 0";
            using(var connection = _context.CreateConnection())
            {
                return (await connection.QueryAsync<TestCase>(sqlQuery,  new { ProblemId = problemId})).ToList();
            }
        }
    }
}
