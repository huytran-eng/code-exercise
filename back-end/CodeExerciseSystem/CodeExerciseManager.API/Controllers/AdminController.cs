using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace CodeExerciseManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginInDTO loginRequest)
        {
            var result = await _adminService.AdminLoginAsync(loginRequest);

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
            else return Ok(result.Data);
        }
    }
}
