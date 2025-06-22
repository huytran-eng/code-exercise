using CodeExercise.Application.DTO.Persistence;
using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;

namespace CodeExercise.Application.Abstractions.IRepositories
{
    public interface ITagRepository
    {
        Task<Guid> AdminCreateTagAsync(TagDTO tag);
        Task<TagDTO> GetTagByNameAsync(string name);
        Task<(List<AdminTagListResponseDTO> Tags, int TotalCount)> GetTagsAsync(AdminTagListInDTO filter);
        Task<AdminTagDetailResponseDTO> AdminGetTagByIdAsync(Guid id);
        Task<Guid> AdminUpdateTagAsync(TagDTO tag);
        Task<bool> CheckListTagsExistAsync(List<Guid> tagIds);
    }
}
