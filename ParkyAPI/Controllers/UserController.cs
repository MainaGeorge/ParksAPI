using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Services.IRepositoryService;

namespace ParkyAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet("{username}", Name = "GetUser")]
        public IActionResult GetUser(string username)
        {
            var user = _userRepository.GetUser(username);

            if (user == null) return NotFound();

            user.Password = "";
            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepository.AuthenticateUser(model.Username, model.Password);

            if (user == null) return BadRequest(new { message = "username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationModel userModel)
        {
            var isUsernameUnique = _userRepository.IsUniqueUser(userModel.Username);

            if (!isUsernameUnique) return BadRequest(new { message = "username already exists" });

            var user = _userRepository.RegisterUser(userModel.Username, userModel.Password);

            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "could not save user" });

            user.Password = "";
            return CreatedAtAction("GetUser", new { username = user.Username, version = HttpContext.GetRequestedApiVersion()?.ToString() }, user);
        }
    }
}
