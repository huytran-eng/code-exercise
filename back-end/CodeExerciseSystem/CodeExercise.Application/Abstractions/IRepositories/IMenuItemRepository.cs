using CodeExercise.Application.DTO.Persistence;

namespace CodeExercise.Application.Abstractions.IRepositories
{
    public interface IMenuItemRepository
    {
        Task<List<MenuItemDTO>> GetMenuItemListForAdmin();
    }
}
