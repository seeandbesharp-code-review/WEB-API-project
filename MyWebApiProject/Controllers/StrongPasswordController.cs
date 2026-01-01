using Entity;
using Microsoft.AspNetCore.Mvc;
using Service;


namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrongPasswordController : ControllerBase
    {
        private readonly ISignUpService _service;

        public StrongPasswordController(ISignUpService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ActionResult<int>> StrongPassword([FromBody] User user)
        {

            return Ok(await _service.StrongPassword(user));
        }
    }
}
