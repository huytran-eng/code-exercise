using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeExerciseManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TagController : Controller
    {
        private ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagDetail(Guid id)
        {
            var result = await _tagService.AdminGetTagDetailAsync(id);
            if (!result.Success)
            {
                return result.StatusCode switch
                {
                    404 => NotFound(result),
                    401 => Unauthorized(result),
                    400 => BadRequest(result),
                    _ => StatusCode(500, result)
                };
            }
            else return Ok(result);
        }

        [HttpPost("get")]
        public async Task<IActionResult> Get(AdminTagListInDTO adminTagListInDTO)
        {
            var result = await _tagService.AdminGetTagsAsync(adminTagListInDTO);
            if (!result.Success)
            {
                return result.StatusCode switch
                {
                    404 => NotFound(result),
                    401 => Unauthorized(result),
                    400 => BadRequest(result),
                    _ => StatusCode(500, result)
                };
            }
            else return Ok(result);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateTag(CreateTagInDTO createTagDTO)
        {
            // get logged in user id
            var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdentifier == null)
            {
                return Unauthorized("Unauthorized");
            }

            var userId = Guid.Parse(userIdentifier);
            var result = await _tagService.AdminCreateTagAsync(createTagDTO, userId);

            if (!result.Success)
            {
                return result.StatusCode switch
                {
                    404 => NotFound(result),
                    401 => Unauthorized(result),
                    400 => BadRequest(result),
                    _ => StatusCode(500, result)
                };
            }
            else return Ok(result);
        }


        [HttpPost("update")]
        public async Task<IActionResult> UpdateTag(AdminUpdateTagInDTO updateTagDTO)
        {
            // get logged in user id
            var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdentifier == null)
            {
                return Unauthorized("Unauthorized");
            }

            var userId = Guid.Parse(userIdentifier);
            var result = await _tagService.AdminUpdateTagAsync(updateTagDTO, userId);

            if (!result.Success)
            {
                return result.StatusCode switch
                {
                    404 => NotFound(result),
                    401 => Unauthorized(result),
                    400 => BadRequest(result),
                    _ => StatusCode(500, result)
                };
            }
            else return Ok(result);
        }
    }
}
