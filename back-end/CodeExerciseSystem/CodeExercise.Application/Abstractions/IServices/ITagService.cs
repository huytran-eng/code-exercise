using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Core.Common;

namespace CodeExercise.Application.Abstractions.IServices
{
    public interface ITagService
    {
        Task<ResponseResult<Guid>> AdminCreateTagAsync(CreateTagInDTO tag,Guid userId );
        Task<ResponseResult<List<AdminTagListResponseDTO>>> AdminGetTagsAsync(AdminTagListInDTO adminTagListInDTO);
        Task<ResponseResult<AdminTagDetailResponseDTO>> AdminGetTagDetailAsync(Guid id);
        Task<ResponseResult<Guid>> AdminUpdateTagAsync(AdminUpdateTagInDTO tag, Guid userId);
    }
}
