using AuthService.Models;
using AuthService.Models.Auth;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public UserController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            if (request == null) return BadRequest("request is null");

            var validateionData = request.IsRequestValid();
            if (!validateionData.valid) return BadRequest(validateionData.errorMessage);

            try
            {
                var user = await _userService.RegisterUserAsync(request);

                return user == null
                    ? BadRequest("can not register a user")
                    : Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(AuthModel authModel)
        {
            if (authModel == null) return BadRequest("authModel is null");

            var validationData = authModel.IsRequestValid();
            if (!validationData.valid) return BadRequest(validationData.errorMessage);

            var token = await _authenticationService.LoginUserAsync(authModel);
            if (token == null) return BadRequest("can not create a token");

            HttpContext.Response.Cookies.Append("zwpat0_1", token.AccessToken);
            return Ok(token);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel == null) return BadRequest("tokenModel is null");
            if (string.IsNullOrEmpty(tokenModel.RefreshToken)) return BadRequest("RefreshToken is null or empty");
            if (string.IsNullOrEmpty(tokenModel.AccessToken)) return BadRequest("AccessToken is null or empty");

            var tokenDto = await _authenticationService.RefreshToken(tokenModel);
            return Ok(tokenDto);
        }

        [HttpGet]
        [Route("{id}/delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0) return BadRequest("id can not be 0 or less");

            await _userService.DeleteUserAsync(id);
            return Ok();
        }

        [HttpPost]
        [Route("{id}/update")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
        {
            if (id <= 0) return BadRequest("id can not be 0 or less");
            if (request == null) return BadRequest("request is null");

            request.Id = id;
            var validateionData = request.IsRequestValid();
            if (!validateionData.valid) return BadRequest(validateionData.errorMessage);

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(request);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
