using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Common;
using Microsoft.Extensions.Logging;

namespace CodeExercise.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ILogger<MenuItemService> _logger;

        public MenuItemService(IMenuItemRepository menuItemRepository, ILogger<MenuItemService> logger)
        {
            _menuItemRepository = menuItemRepository;
            _logger = logger;
        }


        public async Task<ResponseResult<List<AdminMenuItemListResponseDTO>>> GetAdminMenuItemList()
        {
            try
            {
                var res = new ResponseResult<List<AdminMenuItemListResponseDTO>>();
                var menuItems = await _menuItemRepository.GetMenuItemListForAdmin();

                if (menuItems == null || !menuItems.Any())
                {
                    return ResponseResult<List<AdminMenuItemListResponseDTO>>.FailureResponse(404, MessageCodeEnum.RECORDS_NOT_FOUND);
                }

                var menuItemsList = menuItems.Select(dto => new AdminMenuItemListResponseDTO
                {
                    Id = dto.Id,
                    MenuName = dto.MenuName,
                    MenuDisplayName = dto.MenuDisplayName,
                    MenuUrl = dto.MenuUrl,
                    OrderNo = dto.OrderNo,
                    IsActive = dto.IsActive,
                    ParentItemId = dto.ParentItemId,
                    ChildItems = new List<AdminMenuItemListResponseDTO>()
                }).ToList();

                var menuItemDict = menuItemsList.ToDictionary(m => m.Id);

                var rootItems = new List<AdminMenuItemListResponseDTO>();

                foreach (var item in menuItemsList)
                {
                    if (item.ParentItemId == null || item.ParentItemId == Guid.Empty)
                    {
                        rootItems.Add(item);
                    }
                    else if (menuItemDict.TryGetValue(item.ParentItemId.Value, out var parent))
                    {
                        if (parent.ChildItems == null)
                            parent.ChildItems = new List<AdminMenuItemListResponseDTO>();

                        parent.ChildItems = parent.ChildItems.Append(item).ToList();
                    }
                }

                SortMenuItems(rootItems);

                res.Data = rootItems;
                res.Success = true;
                res.StatusCode = 200;
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred when getting tags {ex.Message}");
                return ResponseResult<List<AdminMenuItemListResponseDTO>>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }

        private void SortMenuItems(List<AdminMenuItemListResponseDTO> items)
        {
            items.Sort((a, b) => a.OrderNo.CompareTo(b.OrderNo)); // Sort current level

            foreach (var item in items)
            {
                if (item.ChildItems != null && item.ChildItems.Any())
                {
                    SortMenuItems(item.ChildItems); // Recursive sort
                }
            }
        }
    }
}
