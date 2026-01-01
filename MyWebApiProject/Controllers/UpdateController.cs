using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entity;
using Service;


namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly IUpdateService _service;

        public UpdateController(IUpdateService service)
        {
            _service = service;
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] User user)
        {
           bool? success = await _service.Update(user);
            if (success == null)
                return BadRequest("Invalid user data");
            if (success==true)
                return Ok("User updated successfully");
            return NotFound("User not found");
        }
    }
}
