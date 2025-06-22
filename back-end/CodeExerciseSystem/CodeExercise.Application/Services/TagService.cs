using CodeExercise.Application.Abstractions.IRepositories;
using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.DTO.Response;
using CodeExercise.Application.EntityMapper;
using CodeExercise.Core.Common;
using CodeExercise.Core.Entities;
using CodeExercise.Core.Utils;
using Microsoft.Extensions.Logging;

namespace CodeExercise.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ILogger<TagService> _logger;
        private readonly ITagRepository _tagRepository;
        public TagService(ILogger<TagService> logger, ITagRepository tagRepository)
        {
            _logger = logger;
            _tagRepository = tagRepository;
        }

        /// <summary>
        /// <summary>
        /// Create a new tag
        /// </summary>
        /// <param name="createTagRequestDTO">dto for creating a new tag</param>
        /// <param name="userId">logged in user id</param>
        /// <returns></returns>
        public async Task<ResponseResult<Guid>> AdminCreateTagAsync(CreateTagInDTO createTagRequestDTO, Guid userId)
        {
            try
            {
                // check if tag with given name already exists
                var existingTag = await _tagRepository.GetTagByNameAsync(createTagRequestDTO.Name);
                if (existingTag != null)
                {
                    return ResponseResult<Guid>.FailureResponse(400, MessageCodeEnum.TAG_NAME_EXIST);
                }

                // Create the Tag entity with constructor validation
                var tag = new Tag(createTagRequestDTO.Name, createTagRequestDTO.DisplayName,  createTagRequestDTO.Description, userId);

                // Map to DTO
                var createTagDTO = tag.ToDTO();

                // Insert the tag into the database
                var createdTagId = await _tagRepository.AdminCreateTagAsync(createTagDTO);

                // If tag creation is successful, return the DTO
                return ResponseResult<Guid>.SuccessResponse(createdTagId, 201, MessageCodeEnum.CREATE_TAG_SUCCESSFUL);
            }
            catch (ArgumentWithMessageCodeException ex)
            {
                var message = MessageMapper.GetMessage(ex.MessageCode);
                _logger.LogError(ex, $"Validation failed when creating tag: {message}");

                return ResponseResult<Guid>.FailureResponse(400, ex.MessageCode);
            }
            catch (Exception ex)
            {
                // Log and handle any other exceptions
                _logger.LogError(ex, $"An error occurred when creating a tag {ex.Message}");
                return ResponseResult<Guid>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }

        public async Task<ResponseResult<Guid>> AdminUpdateTagAsync(AdminUpdateTagInDTO updateTagRequestDTO, Guid userId)
        {
            try
            {
                var existingTag = await _tagRepository.AdminGetTagByIdAsync(updateTagRequestDTO.Id);
                if (existingTag == null)
                {
                    return ResponseResult<Guid>.FailureResponse(404, MessageCodeEnum.NOT_FOUND);

                }

                // check if tag with given name already exists
                var tagWithSameName = await _tagRepository.GetTagByNameAsync(updateTagRequestDTO.Name);
                if (tagWithSameName != null && tagWithSameName.Id != updateTagRequestDTO.Id)
                {
                    return ResponseResult<Guid>.FailureResponse(400, MessageCodeEnum.TAG_NAME_EXIST);
                }

                var tagEntity = new Tag
                {
                    Id = existingTag.Id,
                    Name = existingTag.Name,
                    DisplayName = existingTag.DisplayName,
                    CreatedById = existingTag.CreatedById,
                    Description = existingTag.Description,
                    CreatedAt = existingTag.CreatedAt,
                    UpdatedById = existingTag.UpdatedById,
                    UpdatedAt = existingTag.UpdatedAt
                };

                // Update the tag with new values
                tagEntity.Update(updateTagRequestDTO.Name, updateTagRequestDTO.DisplayName, updateTagRequestDTO.Description, userId);


                // Map to DTO
                var updateTagDTO = tagEntity.ToDTO();

                // Insert the tag into the database
                var updatedTagId = await _tagRepository.AdminUpdateTagAsync(updateTagDTO);

                // If tag creation is successful, return the DTO
                return ResponseResult<Guid>.SuccessResponse(updatedTagId, 201, MessageCodeEnum.UPDATE_TAG_SUCCESSFUL);
            }
            catch (ArgumentWithMessageCodeException ex)
            {
                var message = MessageMapper.GetMessage(ex.MessageCode);
                _logger.LogError(ex, $"Validation failed when creating tag: {message}");

                return ResponseResult<Guid>.FailureResponse(400, ex.MessageCode);
            }
            catch (Exception ex)
            {
                // Log and handle any other exceptions
                _logger.LogError(ex, $"An error occurred when creating a tag {ex.Message}");
                return ResponseResult<Guid>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }

        public async Task<ResponseResult<List<AdminTagListResponseDTO>>> AdminGetTagsAsync(AdminTagListInDTO adminTagListInDTO)
        {
            try
            {
                if (adminTagListInDTO.PageSize > 1000)
                {
                    return ResponseResult<List<AdminTagListResponseDTO>>.FailureResponse(400, MessageCodeEnum.RECORDS_MAX_ROW_COUNT);

                }
                var res = new ResponseResult<List<AdminTagListResponseDTO>>();
                var (tags, totalCount) = await _tagRepository.GetTagsAsync(adminTagListInDTO);
                if (tags == null || !tags.Any())
                {
                    return ResponseResult<List<AdminTagListResponseDTO>>.FailureResponse(404, MessageCodeEnum.RECORDS_NOT_FOUND);
                }
                else
                {
                    res.Data = tags;
                    res.Success = true;
                    res.TotalCount = totalCount;
                    res.StatusCode = 200;
                    res.PageNumber = adminTagListInDTO.PageNumber;
                    res.PageCount = (int)Math.Ceiling((double)totalCount / adminTagListInDTO.PageSize);
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred when getting tags {ex.Message}");
                return ResponseResult<List<AdminTagListResponseDTO>>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }


        public async Task<ResponseResult<AdminTagDetailResponseDTO>> AdminGetTagDetailAsync(Guid id)
        {
            try
            {
                var tag = await _tagRepository.AdminGetTagByIdAsync(id);
                if (tag == null)
                {
                    return ResponseResult<AdminTagDetailResponseDTO>.FailureResponse(404, MessageCodeEnum.NOT_FOUND);
                }
                else
                {
                    return ResponseResult<AdminTagDetailResponseDTO>.SuccessResponse(tag, 200, MessageCodeEnum.SUCCESS);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred when getting tag detail {ex.Message}");
                return ResponseResult<AdminTagDetailResponseDTO>.FailureResponse(500, MessageCodeEnum.SYSTEM_FAILURE);
            }
        }
    }
}
