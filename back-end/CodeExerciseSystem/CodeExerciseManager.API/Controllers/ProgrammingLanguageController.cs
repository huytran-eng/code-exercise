using CodeExercise.Application.Abstractions.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeExerciseManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProgrammingLanguage : Controller
    {
        private IProgrammingLanguageService _programmingLanguageService;

        public ProgrammingLanguage(IProgrammingLanguageService programmingLanguageService)
        {
            _programmingLanguageService = programmingLanguageService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var result = await _programmingLanguageService.AdminGetAllProgrammingLanguagesAsync();
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
