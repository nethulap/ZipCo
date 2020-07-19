using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZipPayApi.Model;
using ZipPayApi.Repository;

namespace ZipPayApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private const string DUPLICATE_EMAIL_ERROR = "This email address is already in use";
        private const string USER_CREATION_SUCCESSFULL = "User is successfully created";

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserAsync(int id)
        {
            var user = await _userRepository.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> PostUserAsync([FromBody] UserRequest user)
        {
            if (ModelState.IsValid)
            {
                var isExistingUser = await _userRepository.UserExistsAsync(user.Email);
                if (!isExistingUser)
                {
                    var newUser = await _userRepository.CreateUserAsync(user);
                    _logger.LogInformation(USER_CREATION_SUCCESSFULL);
                    return CreatedAtAction("GetUser", new { id = user.Id }, newUser);
                }
                else
                {
                    _logger.LogError(DUPLICATE_EMAIL_ERROR + ", {@user}", user);
                    return BadRequest(DUPLICATE_EMAIL_ERROR);
                }
            }

            return BadRequest();
        }
    }
}
