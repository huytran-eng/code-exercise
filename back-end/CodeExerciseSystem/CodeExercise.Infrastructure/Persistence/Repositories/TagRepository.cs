using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;
using Dapper;
using System.Text;

namespace CodeExercise.Infrastructure.Persistence.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DapperContext _context;
        public TagRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<(List<AdminTagListResponseDTO> Tags, int TotalCount)> GetTagsAsync(AdminTagListInDTO filter)
        {
            var parameters = new DynamicParameters();

            var whereClause = new StringBuilder("WHERE 1=1");

            // query string builder based on filter parameters
            if (!string.IsNullOrEmpty(filter.Name))
            {
                whereClause.Append(" AND t.Name = @Name");
                parameters.Add("Name", filter.Name);
            }

            if (!string.IsNullOrEmpty(filter.DisplayName))
            {
                whereClause.Append(" AND t.DisplayName = @DisplayName");
                parameters.Add("DisplayName", filter.DisplayName);
            }

            if (filter.FromDate.HasValue)
            {
                whereClause.Append(" AND t.CreatedAt >= @FromDate");
                parameters.Add("FromDate", filter.FromDate);
            }

            if (filter.ToDate.HasValue)
            {
                whereClause.Append(" AND t.CreatedAt <= @ToDate");
                parameters.Add("ToDate", filter.ToDate);
            }

            whereClause.Append(" AND t.IsDeleted = 0");

            var sqlBuilder = new StringBuilder(@"
                SELECT 
                    t.Id, t.Name, t.DisplayName, t.CreatedAt, t.UpdatedAt, 
                    t.CreatedById, t.UpdatedById, 
                    u1.Username AS CreatedByName, 
                    u2.Username AS UpdatedByName 
                FROM Tag t
                LEFT JOIN [User] u1 ON t.CreatedById = u1.Id
                LEFT JOIN [User] u2 ON t.UpdatedById = u2.Id
            ");

            sqlBuilder.Append(whereClause);
            string orderByClause = " ORDER BY ";

            switch (filter.OrderBy)
            {
                case 1:
                    orderByClause += "t.Name ASC";
                    break;
                case 2:
                    orderByClause += "t.DisplayName ASC";
                    break;
                case 3:
                    orderByClause += "t.CreatedAt DESC";
                    break;
                case 4:
                    orderByClause += "t.UpdatedAt DESC";
                    break;
                default:
                    orderByClause += "t.CreatedAt DESC"; // Default fallback
                    break;
            }


            sqlBuilder.Append(orderByClause);
            sqlBuilder.Append(" OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY");

            int skip = (filter.PageNumber - 1) * filter.PageSize;
            parameters.Add("Skip", skip);
            parameters.Add("Take", filter.PageSize);

            var countBuilder = new StringBuilder(@"
                SELECT COUNT(*) 
                FROM Tag t
                LEFT JOIN [User] u1 ON t.CreatedById = u1.Id
                LEFT JOIN [User] u2 ON t.UpdatedById = u2.Id
            ");
            countBuilder.Append(whereClause);

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(
                sqlBuilder.ToString() + ";" + countBuilder.ToString(),
                parameters
            );

            var tags = (await multi.ReadAsync<AdminTagListResponseDTO>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return (tags, totalCount);
        }

        public async Task<Guid> AdminCreateTagAsync(TagDTO tag)
        {
            string sql = "INSERT INTO Tag (Id, Name, DisplayName, Description, CreatedAt, CreatedById) VALUES (@Id, @Name, @DisplayName, @Description, @CreatedAt, @CreatedById);";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, tag);
            }

            return tag.Id;
        }

        public async Task<TagDTO> GetTagByNameAsync(string name)
        {
            string sql = "SELECT * FROM Tag WHERE Name = @Name AND IsDeleted = 0;";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<TagDTO>(sql, new { Name = name });
            }
        }

        public async Task<AdminTagDetailResponseDTO> AdminGetTagByIdAsync(Guid id)
        {
            string sql = @"
                SELECT 
                    t.Id, t.Name, t.DisplayName, t.Description, t.CreatedAt, t.UpdatedAt, 
                    t.CreatedById, t.UpdatedById, 
                    u1.Username AS CreatedByName, 
                    u2.Username AS UpdatedByName 
                FROM Tag t
                LEFT JOIN [User] u1 ON t.CreatedById = u1.Id
                LEFT JOIN [User] u2 ON t.UpdatedById = u2.Id
                WHERE t.Id = @Id
                AND t.IsDeleted = 0
            ";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<AdminTagDetailResponseDTO>(sql, new { Id = id });
            }
        }

        public async Task<Guid> AdminUpdateTagAsync(TagDTO tag)
        {
            string sql = @"
            UPDATE Tag SET  [Name] = @Name, 
				            [DisplayName] = @DisplayName,
                            [Description] = @Description,
				            [UpdatedAt] = @UpdatedAt,
				            [UpdatedById] = @UpdatedById
	            WHERE Id = @Id AND IsDeleted = 0";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, tag);
            }

            return tag.Id;
        }


        /// <summary>
        /// Check if all element of a given list of tag ids exist in the database
        /// </summary>
        /// <param name="tagIds">given list of tags id</param>
        /// <returns></returns>
        public async Task<bool> CheckListTagsExistAsync(List<Guid> tagIds)
        {
            string sql = @"SELECT COUNT(*) FROM Tag t  WHERE t.Id IN @TagIds AND t.IsDeleted = 0";
            using (var connection = _context.CreateConnection())
            {
                var existingCount = await connection.ExecuteScalarAsync<int>(sql, new {TagIds = tagIds });
                return existingCount == tagIds.Count;
            }
        }

    }
}
