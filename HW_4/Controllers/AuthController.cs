using HW_4.Contracts;
using HW_4.Services;
using Microsoft.AspNetCore.Mvc;

namespace HW_4.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IUserService users) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserResponse>> Login(
        LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await users.AuthorizeAsync(request, cancellationToken);
        return user is null ? Unauthorized() : Ok(user);
    }
}
