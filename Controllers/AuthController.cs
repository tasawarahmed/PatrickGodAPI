using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatrickGodAPI.Dtos.User;

namespace PatrickGodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        public AuthController(IAuthRepository authRepo)
        {
            this.authRepository = authRepo;            
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register (UserRegisterDto request)
        {
            var response = await authRepository.Register(
                new User { Username = request.Username }, request.Password
                );
            if (!response.Success) 
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
        {
            var response = await authRepository.Login(
                request.Username, request.Password
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


    }
}
