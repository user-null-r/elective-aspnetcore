using HW_4.Contracts;
using HW_4.Services;
using Microsoft.AspNetCore.Mvc;

namespace HW_4.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(IUserService users) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<UserResponse>> Create(
        CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await users.CreateAsync(request, cancellationToken);
        return Created($"/api/users/{result.Id}", result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> Update(
        Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        return Ok(await users.UpdateAsync(id, request, cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await users.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<UserResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(
        [FromQuery] UserDateFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await users.GetAllAsync(filter, cancellationToken));
    }
}

