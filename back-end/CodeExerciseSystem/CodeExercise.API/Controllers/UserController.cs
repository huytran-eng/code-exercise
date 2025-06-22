using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.DTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace CodeExercise.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInDTO loginRequest)
        {
            var result = await _userService.LoginAsync(loginRequest);

            if (!result.Success)
            {
                return result.StatusCode switch
                {
                    404 => NotFound(result.Message),
                    401 => Unauthorized(result.Message),
                    _ => BadRequest(result.Message)
                };
            }
            else return Ok(result.Data);
        }
    }
}
