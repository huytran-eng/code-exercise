using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Common;

namespace CodeExercise.Application.Abstractions.IServices
{
    public interface IMenuItemService
    {
        Task<ResponseResult<List<AdminMenuItemListResponseDTO>>> GetAdminMenuItemList();
    }
}
