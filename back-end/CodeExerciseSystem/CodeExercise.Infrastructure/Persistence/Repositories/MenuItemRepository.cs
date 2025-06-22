using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Application.DTO.Response;
using Dapper;

namespace CodeExercise.Infrastructure.Persistence.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly DapperContext _context;
        public MenuItemRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItemDTO>> GetMenuItemListForAdmin()
        {
            string sql = "SELECT Id, MenuName, MenuUrl, MenuDisplayName, OrderNo, ParentItemId, IsActive" +
                " FROM MenuItem" +
                " Where IsDeleted = 0";

            using (var connection = _context.CreateConnection())
            {
                var menuItems = (await connection.QueryAsync<MenuItemDTO>(sql)).ToList();

                return menuItems;
            }
        }
    }
}
