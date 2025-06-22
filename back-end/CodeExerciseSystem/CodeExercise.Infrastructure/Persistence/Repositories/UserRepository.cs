using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.DTO.Persistence;
using Dapper;

namespace CodeExercise.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT * FROM [User] u WHERE Username = @Username AND Position = 0";
            var user = await connection.QuerySingleOrDefaultAsync<UserDTO>(query, new { Username = username });
            return user ;
        }

        public async Task<UserDTO> GetAdminByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT * FROM [User] u WHERE Username = @Username AND Position = 1";
            var user = await connection.QuerySingleOrDefaultAsync<UserDTO>(query, new { Username = username });
            return user;
        }
    }
}
