using Azure.Core;
using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Request;
using CodeExercise.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeExerciseManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProblemController : Controller
    {
        private readonly IProblemService _problemService;
        public ProblemController(IProblemService problemService)
        {
            _problemService = problemService;
        }

        [HttpPost("get")]
        public async Task<IActionResult> Get(AdminProblemListInDTO adminProblemListInDTO)
        {
            var result = await _problemService.AdminGetProblemsAsync(adminProblemListInDTO);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProblemDetail(Guid id)
        {
            var result = await _problemService.AdminGetProblemDetailAsync(id);
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

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteProblem([FromBody] AdminDeleteProblemInDTO request)
        {
            var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdentifier == null)
            {
                return Unauthorized("Unauthorized");
            }

            var userId = Guid.Parse(userIdentifier);
            var result = await _problemService.AdminDeleteProblemAsync(request.Id, userId);
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

        /// <summary>
        /// api to update a problem
        /// </summary>
        /// <param name="problemDTO"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> UpdateProblem([FromBody] AdminUpdateProblemInDTO problemDTO)
        {
            var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdentifier == null)
            {
                return Unauthorized("Unauthorized");
            }

            var userId = Guid.Parse(userIdentifier);
            var result = await _problemService.AdminUpdateProblemAsync(problemDTO, userId);

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


        /// <summary>
        /// api to create a new problem
        /// </summary>
        /// <param name="problemDTO"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateProblem([FromBody] AdminCreateProblemInDTO problemDTO)
        {
            var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdentifier == null)
            {
                return Unauthorized("Unauthorized");
            }

            var userId = Guid.Parse(userIdentifier);
            var result = await _problemService.AdminCreateProblemAsync(problemDTO, userId);

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
