using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entity;
using Service;
using System.Security.Cryptography.X509Certificates;




namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly ISignInService _service;

        public SignInController(ISignInService service)
        {
            _service = service;
           
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SignIn signInRequest)
        {
            User? authenticatedUser = await _service.SignIn(signInRequest);
            if (authenticatedUser == null)
                return Unauthorized("Invalid credentials");
            return Ok(authenticatedUser);
            
        }
    }
}
