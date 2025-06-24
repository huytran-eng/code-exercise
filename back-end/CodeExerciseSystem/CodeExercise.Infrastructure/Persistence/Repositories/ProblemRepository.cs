using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Entities;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CodeExercise.Infrastructure.Persistence.Repositories
{
    public class ProblemRepository : IProblemRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<ProblemRepository> _logger;

        public ProblemRepository(DapperContext context, ILogger<ProblemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(List<AdminProblemListResponseDTO> Problems, int TotalCount)> AdminGetProblemsAsync(AdminProblemListInDTO filter)
        {
            var parameters = new DynamicParameters();

            var whereClause = new StringBuilder("WHERE 1=1");

            // query string builder based on filter parameters
            if (!string.IsNullOrEmpty(filter.Title))
            {
                whereClause.Append(" AND p.Title = @Title");
                parameters.Add("Title", filter.Title);
            }

            if (filter.FromDate.HasValue)
            {
                whereClause.Append(" AND p.CreatedAt >= @FromDate");
                parameters.Add("FromDate", filter.FromDate);
            }

            if (filter.ToDate.HasValue)
            {
                whereClause.Append(" AND p.CreatedAt <= @ToDate");
                parameters.Add("ToDate", filter.ToDate);
            }

            whereClause.Append(" AND p.IsDeleted = 0");

            var sqlBuilder = new StringBuilder(@"
                SELECT 
                    p.Id, p.Title, p.Difficulty, p.TimeLimit, p.MemoryLimit, p.CreatedAt, p.UpdatedAt, 
                    p.CreatedById, p.UpdatedById, 
                    u1.Username AS CreatedByName, 
                    u2.Username AS UpdatedByName 
                FROM Problem p
                LEFT JOIN [User] u1 ON p.CreatedById = u1.Id
                LEFT JOIN [User] u2 ON p.UpdatedById = u2.Id
            ");

            sqlBuilder.Append(whereClause);
            string orderByClause = " ORDER BY ";

            switch (filter.OrderBy)
            {
                case 1:
                    orderByClause += "p.Name ASC";
                    break;
                case 2:
                    orderByClause += "p.DisplayName ASC";
                    break;
                case 3:
                    orderByClause += "p.CreatedAt DESC";
                    break;
                case 4:
                    orderByClause += "p.UpdatedAt DESC";
                    break;
                default:
                    orderByClause += "p.CreatedAt DESC"; // Default fallback
                    break;
            }


            sqlBuilder.Append(orderByClause);
            sqlBuilder.Append(" OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY");

            int skip = (filter.PageNumber - 1) * filter.PageSize;
            parameters.Add("Skip", skip);
            parameters.Add("Take", filter.PageSize);

            var countBuilder = new StringBuilder(@"
                SELECT COUNT(*) 
                FROM Problem p
                LEFT JOIN [User] u1 ON p.CreatedById = u1.Id
                LEFT JOIN [User] u2 ON p.UpdatedById = u2.Id
            ");
            countBuilder.Append(whereClause);

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(
                sqlBuilder.ToString() + ";" + countBuilder.ToString(),
                parameters
            );

            var problems = (await multi.ReadAsync<AdminProblemListResponseDTO>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return (problems, totalCount);
        }


        public async Task<Guid> AdminCreateProblemAsync(Problem problem, List<TestCase> testCases, List<TemplateCode> templateCodes)
        {
            using var connection = _context.CreateConnection();
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                try
                {
                    var insertProblemSql = @"INSERT INTO Problem (Id, Title, Description, Difficulty, TimeLimit, MemoryLimit, Slug, CreatedById, CreatedAt)
                                VALUES (@Id, @Title, @Description, @Difficulty, @TimeLimit, @MemoryLimit, @Slug, @CreatedById, @CreatedAt);";
                    await connection.ExecuteAsync(insertProblemSql, problem, transaction);

                    const string insertTestCasesSql = @"
                                INSERT INTO TestCase (Id, Input, ExpectedOutput, InputDisplay, ExpectedOutputDisplay, IsHidden, ProblemId, CreatedAt, CreatedById)
                                VALUES (@Id, @Input, @ExpectedOutput, @InputDisplay, @ExpectedOutputDisplay, @IsHidden, @ProblemId, @CreatedAt, @CreatedById);";

                    await connection.ExecuteAsync(insertTestCasesSql, testCases, transaction);

                    const string insertTemplateCodesSql = @"
                                INSERT INTO TemplateCode (Id, UserTemplateCode, HiddenTemplateCode, ProgrammingLanguageId,  ProblemId, CreatedAt, CreatedById)
                                VALUES (@Id, @UserTemplateCode, @HiddenTemplateCode, @ProgrammingLanguageId, @ProblemId, @CreatedAt, @CreatedById);";
                    await connection.ExecuteAsync(insertTemplateCodesSql, templateCodes, transaction);

                    //const string insertProblemTagSql = @"
                    //            INSERT INTO ProblemTag (Id, ProblemId, TagId, CreatedAt, CreatedById) 
                    //            VALUES (@Id, @ProblemId, @TagId, @CreatedAt, @CreatedById);";
                    //await connection.ExecuteAsync(insertProblemTagSql, problemTags, transaction);

                    transaction.Commit();
                    return problem.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// find a problem with all related list and objects ( test cases, template codes, tags)
        /// </summary>
        /// <param name="problemId"></param>
        /// <returns></returns>
        public async Task<ProblemDTO> AdminGetProblemDetailByIdAsync(Guid problemId)
        {
            // query for problem, template code, test case, problem tag tables for problem with given id
            //string query = @"SELECT [Id]
            //                          ,[Title]
            //                          ,[Description]
            //                          ,[Difficulty]
            //                          ,[TimeLimit]
            //                          ,[MemoryLimit]
            //                          ,[Slug]
            //                          ,[UpdatedById]
            //                          ,[UpdatedAt]
            //                          ,[CreatedById]
            //                          ,[CreatedAt]
            //                          ,[IsDeleted]
            //                          ,[DeletedAt]
            //                          ,[DeletedById]
            //                        FROM [Problem] 
            //                        WHERE Id = @Id AND IsDeleted = 0 ;

            //                        SELECT tplc.[Id]
            //                            ,tplc.[Code]
            //                            ,tplc.[ProblemId]
            //                            ,tplc.[ProgrammingLanguageId]
            //                            ,pl.[Name] AS ProgrammingLanguageName
            //                            ,tplc.[CreatedAt]
            //                            ,tplc.[CreatedById]
            //                            ,tplc.[UpdatedAt]
            //                            ,tplc.[UpdatedById]
            //                            ,tplc.[IsDeleted]
            //                            ,tplc.[DeletedAt]
            //                            ,tplc.[DeletedById]
            //                        FROM [ProblemTemplateCode] tplc
            //                        INNER JOIN [Problem] p ON p.Id = tplc.ProblemId
            //INNER JOIN ProgrammingLanguage pl ON tplc.ProgrammingLanguageId = pl.Id
            //                        WHERE p.Id = @Id AND tplc.IsDeleted = 0 AND p.IsDeleted = 0 AND pl.IsDeleted = 0;

            //                        SELECT tc.[Id]
            //                              ,tc.[Input]
            //                              ,tc.[ExpectedOutput]
            //                              ,tc.[IsHidden]
            //                              ,tc.[InputDisplay]
            //                              ,tc.[ExpectedOutputDisplay]
            //                              ,tc.[ProblemId]
            //                              ,tc.[CreatedAt]
            //                              ,tc.[CreatedById]
            //                              ,tc.[UpdatedAt]
            //                              ,tc.[UpdatedById]
            //                              ,tc.[IsDeleted]
            //                              ,tc.[DeletedAt]
            //                              ,tc.[DeletedById]
            //                          FROM [TestCase] tc
            //                          INNER JOIN [Problem] p ON p.Id = tc.ProblemId
            //                          WHERE p.Id = @Id AND tc.IsDeleted = 0 AND p.IsDeleted = 0;

            //                        SELECT pt.[Id]
            //                              ,pt.[ProblemId]
            //                              ,pt.[TagId]
            //                              ,t.[Name] as TagName
            //                              ,pt.[CreatedAt]
            //                              ,pt.[CreatedById]
            //                              ,pt.[UpdatedAt]
            //                              ,pt.[UpdatedById]
            //                              ,pt.[IsDeleted]
            //                              ,pt.[DeletedAt]
            //                              ,pt.[DeletedById]
            //                         FROM [ProblemTag] pt
            //                          INNER JOIN [Problem] p ON pt.ProblemId = p.Id
            //                          INNER JOIN [Tag] t ON pt.TagId = t.Id 
            //                          WHERE p.Id = '3615DECB-7460-441B-81FA-653A3AA314B4' AND p.IsDeleted = 0 AND pt.IsDeleted = 0 AND t.IsDeleted = 0
            //            ";

            string query = @"SELECT [Id]
                                      ,[Title]
                                      ,[Description]
                                      ,[Difficulty]
                                      ,[TimeLimit]
                                      ,[MemoryLimit]
                                      ,[Slug]
                                      ,[UpdatedById]
                                      ,[UpdatedAt]
                                      ,[CreatedById]
                                      ,[CreatedAt]
                                      ,[IsDeleted]
                                      ,[DeletedAt]
                                      ,[DeletedById]
                            FROM [Problem] 
                            WHERE Id = @Id AND IsDeleted = 0 ;

                            SELECT tc.[Id]
                                          ,tc.[Input]
                                          ,tc.[ExpectedOutput]
                                          ,tc.[IsHidden]
                                          ,tc.[InputDisplay]
                                          ,tc.[ExpectedOutputDisplay]
                                          ,tc.[ProblemId]
                                          ,tc.[CreatedAt]
                                          ,tc.[CreatedById]
                                          ,tc.[UpdatedAt]
                                          ,tc.[UpdatedById]
                                          ,tc.[IsDeleted]
                                          ,tc.[DeletedAt]
                                          ,tc.[DeletedById]
                            FROM [TestCase] tc
                                      INNER JOIN [Problem] p ON p.Id = tc.ProblemId
                            WHERE p.Id = @Id AND tc.IsDeleted = 0 AND p.IsDeleted = 0;

                            SELECT	tplc.[Id]
		                            ,tplc.[UserTemplateCode]
		                            ,tplc.[HiddenTemplateCode]
		                            ,tplc.[ProgrammingLanguageId]
		                            ,pl.[Name] as ProgrammingLanguageName
		                            ,pl.[DisplayName] as ProgrammingLanguageDisplayName
		                            ,tplc.[ProblemId]
		                            ,tplc.[CreatedAt]
		                            ,tplc.[CreatedById]
		                            ,tplc.[UpdatedAt]
		                            ,tplc.[UpdatedById]
		                            ,tplc.[IsDeleted]
		                            ,tplc.[DeletedAt]
		                            ,tplc.[DeletedById]
                            FROM TemplateCode tplc
	                            INNER JOIN [Problem] p ON p.Id = tplc.ProblemId
	                            INNER JOIN [ProgrammingLanguage] pl on pl.Id = tplc.ProgrammingLanguageId
                            WHERE p.Id = @Id AND tplc.IsDeleted = 0 AND p.IsDeleted = 0 AND pl.IsDeleted = 0";
            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(query, new { Id = problemId });

            var problem = await multi.ReadSingleOrDefaultAsync<ProblemDTO>();
            if (problem == null)
            {
                return null;
            }

            var testCases = (await multi.ReadAsync<TestCaseDTO>()).ToList();
            var templateCodes = (await multi.ReadAsync<TemplateCodeDTO>()).ToList();
            //var tags = (await multi.ReadAsync<ProblemTagDTO>()).ToList();

            problem.TestCases = testCases;
            problem.TemplateCodes = templateCodes;
            //problem.ProblemTags = tags;

            return problem;
        }

        public async Task<bool> AdminUpdateProblemAsync(Problem problem, List<TestCase> updateTestCases, List<TestCase> newTestCases, List<TemplateCode> updateTemplateCodes, List<TemplateCode> newTemplateCodes)
        {
            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var updateProblemQuery = @"UPDATE Problem
                                       SET Title = @Title,
                                           Description = @Description,
                                           Difficulty = @Difficulty,
                                           TimeLimit = @TimeLimit,
                                           MemoryLimit = @MemoryLimit,
                                           Slug = @Slug,
                                           UpdatedAt = @UpdatedAt,
                                           UpdatedById = @UpdatedById
                                        WHERE Id = @Id;
                                       "
                ;

                await connection.ExecuteAsync(updateProblemQuery, problem, transaction);

                var updateTestCaseQuery =
                   @"Update TestCase" +
                    "   SET Input = @Input," +
                    "       ExpectedOutput = @ExpectedOutput," +
                    "       InputDisplay = @InputDisplay," +
                    "       ExpectedOutputDisplay = @ExpectedOutputDisplay," +
                    "       IsHidden = @IsHidden," +
                    "       UpdatedAt = @UpdatedAt," +
                    "       UpdatedById = @UpdatedById," +
                    "       IsDeleted = @IsDeleted," +
                    "       DeletedById = @DeletedById" +
                    "   WHERE Id = @Id;";
                await connection.ExecuteAsync(updateTestCaseQuery, updateTestCases, transaction);

                var insertTestCaseQuery =
                   @"INSERT INTO TestCase (Id, Input, ExpectedOutput, InputDisplay, ExpectedOutputDisplay, IsHidden, ProblemId, CreatedAt, CreatedById)
                                VALUES (@Id, @Input, @ExpectedOutput, @InputDisplay, @ExpectedOutputDisplay, @IsHidden, @ProblemId, @CreatedAt, @CreatedById);";
                await connection.ExecuteAsync(insertTestCaseQuery, newTestCases, transaction);

                var updateTemplateCodeQuery =
                      @"Update TemplateCode" +
                    "   SET UserTemplateCode = @UserTemplateCode," +
                    "       HiddenTemplateCode = @HiddenTemplateCode," +
                    "       UpdatedAt = @UpdatedAt," +
                    "       UpdatedById = @UpdatedById," +
                    "       IsDeleted = @IsDeleted," +
                    "       DeletedById = @DeletedById" +
                    "   WHERE Id = @Id;";
                await connection.ExecuteAsync(updateTemplateCodeQuery, updateTemplateCodes, transaction);

                var insertTemplateCodeQuery =
                    @"INSERT INTO TestCase (Id, HiddenTemplateCode, ProgrammingLanguageId , ProblemId, CreatedAt, CreatedById)
                                VALUES (@Id, @HiddenTemplateCode, @ProgrammingLanguageId , @ProblemId, @CreatedAt, @CreatedById);";

                await connection.ExecuteAsync(insertTemplateCodeQuery, newTemplateCodes, transaction);

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> AdminDeleteProblemAsync(Problem problem)
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            var deleteProblemQuery = @"UPDATE Problem SET IsDeleted = @IsDeleted,
                                                          DeletedAt = @DeletedAt,
                                                          DeletedById = @DeletedById WHERE Id = @Id ";

            try
            {
                var result = await connection.ExecuteAsync(deleteProblemQuery, problem);
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error when delete problem in repository {ex.Message}");
                return false;
            }
        }


        public async Task<(List<UserProblemListResponseDTO> Problems, int TotalCount)> UserGetProblemsAsync(UserProblemListInDTO filter, Guid? userId)
        {
            var parameters = new DynamicParameters();

            var whereClause = new StringBuilder("WHERE 1=1");

            // query string builder based on filter parameters
            if (!string.IsNullOrEmpty(filter.Title))
            {
                whereClause.Append(" AND p.Title LIKE @Title");
                parameters.Add("Title", $"%{filter.Title}%");
            }

            whereClause.Append(" AND p.IsDeleted = 0");


            var sqlBuilder = new StringBuilder(@"
                                                    SELECT 
                                                        p.Id, 
                                                        p.Title, 
                                                        p.Difficulty,
                                                        p.Slug,
                                                        CASE 
                                                ");

            if (userId != null)
            {
                parameters.Add("UserId", userId);

                sqlBuilder.Append(@"
                            WHEN EXISTS (
                                SELECT 1 
                                FROM UserSubmission us 
                                WHERE us.ProblemId = p.Id AND us.SubmitById = @UserId AND us.[Status] = 1
                            ) THEN 1
                            WHEN EXISTS (
                                SELECT 1 
                                FROM UserSubmission us 
                                WHERE us.ProblemId = p.Id AND us.SubmitById = @UserId
                            ) THEN 2
                ");
            }
            else
            {
                sqlBuilder.Append(@"
                            WHEN 1=1 THEN 0
                ");
            }

            sqlBuilder.Append(@"
                        ELSE 0
                    END AS Status
                FROM Problem p
            ");

            sqlBuilder.Append(whereClause);
            string orderByClause = " ORDER BY ";

            switch (filter.OrderBy)
            {
                case 1:
                    orderByClause += "p.Name ASC";
                    break;
                default:
                    orderByClause += "p.CreatedAt DESC"; // Default fallback
                    break;
            }


            sqlBuilder.Append(orderByClause);
            sqlBuilder.Append(" OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY");

            int skip = (filter.PageNumber - 1) * filter.PageSize;
            parameters.Add("Skip", skip);
            parameters.Add("Take", filter.PageSize);

            var countBuilder = new StringBuilder(@"
                SELECT COUNT(*) 
                FROM Problem p
            ");
            countBuilder.Append(whereClause);

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(
                sqlBuilder.ToString() + ";" + countBuilder.ToString(),
                parameters
            );

            var problems = (await multi.ReadAsync<UserProblemListResponseDTO>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return (problems, totalCount);
        }

        public async Task<UserProblemDetailResponseDTO> UserGetProblemDetailBySlugAsync(string slug)
        {
            string query = @"SELECT [Id]
                                      ,[Title]
                                      ,[Description]
                                      ,[Difficulty]
                                      ,[Slug]
                                    FROM [Problem] 
                                    WHERE [Slug] = @Slug AND IsDeleted = 0 ;
                            SELECT tc.[Id]
                                          ,tc.[InputDisplay]
                                          ,tc.[ExpectedOutputDisplay]
                                      FROM [TestCase] tc
                                      INNER JOIN [Problem] p ON p.Id = tc.ProblemId
                                      WHERE p.Slug = @Slug AND tc.IsDeleted = 0 AND p.IsDeleted = 0 AND tc.IsHidden = 0;
                            SELECT	tplc.[Id]
		                            ,tplc.[UserTemplateCode]
		                            ,tplc.[ProgrammingLanguageId]
		                            ,pl.[DisplayName] as ProgrammingLanguageDisplayName
		                            ,pl.[Name] as ProgrammingLanguageName
                            FROM TemplateCode tplc
	                            INNER JOIN [Problem] p ON p.Id = tplc.ProblemId
	                            INNER JOIN [ProgrammingLanguage] pl on pl.Id = tplc.ProgrammingLanguageId
                            WHERE p.Slug = @Slug AND tplc.IsDeleted = 0 AND p.IsDeleted = 0 ";
                                    
            try
            {
                using var connection = _context.CreateConnection();
                using var multi = await connection.QueryMultipleAsync(query, new { Slug = slug });

                var problem = await multi.ReadSingleOrDefaultAsync<UserProblemDetailResponseDTO>();
                if (problem == null)
                {
                    return null;
                }

                var testCases = (await multi.ReadAsync<UserProblemDetailTestCasesDTO>()).ToList();
                var templateCodes = (await multi.ReadAsync<UserProblemDetailTemplateCodeDTO>()).ToList();

                problem.TemplateCodes = templateCodes;
                problem.TestCases = testCases;

                return problem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error when getting problem detail in repository {ex.Message}");
                return null;
            }

        }
    }
}
