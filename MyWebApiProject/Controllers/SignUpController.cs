using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entity;
using Service;
using System.Runtime.Intrinsics.X86;



namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly ISignUpService _service;

        public SignUpController(ISignUpService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {

            User? createdUser  = await  _service.SignUp(user);
            if (createdUser == null)
                return BadRequest("Registration failed - invalid data or weak password");
            return CreatedAtAction(nameof(Post), new { id = createdUser?.UserId }, createdUser);

        }
    }
}

