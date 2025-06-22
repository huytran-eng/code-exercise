using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Application.DTO.Response;
using Dapper;

namespace CodeExercise.Infrastructure.Persistence.Repositories
{
    public class ProgrammingLanguageRepository : IProgrammingLanguageRepository
    {
        private readonly DapperContext _context;
        public ProgrammingLanguageRepository(DapperContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all the available programming languages
        /// </summary>
        /// <returns></returns>
        public async Task<List<AdminProgrammingLanguageListResponseDTO>> AdminGetAllProgrammingLanguagesAsync()
        {
            string sql = "SELECT * FROM ProgrammingLanguage WHERE IsDeleted = 0;";
            using (var connection = _context.CreateConnection())
            {
                return (await connection.QueryAsync<AdminProgrammingLanguageListResponseDTO>(sql)).ToList();
            }
        }

        /// <summary>
        /// Check if all element of a given list of programming language ids exist in the database
        /// </summary>
        /// <param name="programmingLanguageIds">given list of programming language id</param>
        /// <returns></returns>
        public async Task<bool> CheckListProgrammingLanguageIdsExistAsync(List<Guid> programmingLanguageIds)
        {
            string sql = @"SELECT COUNT(*) FROM ProgrammingLanguage pl WHERE pl.Id IN @ProgrammingLanguageIds AND pl.IsDeleted = 0";
            using (var connection = _context.CreateConnection())
            {
                var existingCount = await connection.ExecuteScalarAsync<int>(sql, new { ProgrammingLanguageIds = programmingLanguageIds });
                return existingCount == programmingLanguageIds.Count;
            }
        }
    }
}
