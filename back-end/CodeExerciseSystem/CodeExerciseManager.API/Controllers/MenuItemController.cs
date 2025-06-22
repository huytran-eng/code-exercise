using CodeExercise.Application.Abstractions.IServices;
using CodeExercise.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CodeExerciseManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var result = await _menuItemService.GetAdminMenuItemList();

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
