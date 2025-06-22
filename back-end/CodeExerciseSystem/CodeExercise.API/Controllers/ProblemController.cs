using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeExercise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly IProblemService _problemService;

        public ProblemController(IProblemService problemSerive)
        {
            _problemService = problemSerive;
        }

        [HttpPost("list")]
        public async Task<IActionResult> ProblemList([FromBody] UserProblemListInDTO problemFilter)
        {
            Guid? userId = null;

            var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdentifier) && Guid.TryParse(userIdentifier, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var result = await _problemService.UserGetProblemsAsync(problemFilter, userId);
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

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProblemDetail(string slug)
        {
            var result = await _problemService.UserGetProblemDetailAsync(slug);
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
