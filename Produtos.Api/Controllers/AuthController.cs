using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Produtos.Application.Auth;

namespace Produtos.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(AuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType<AuthResult>(StatusCodes.Status201Created)]
    public async Task<ActionResult<AuthResult>> Register(
        [FromBody] CredentialsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(new RegisterInput(request.Email, request.Password), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<AuthResult>(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResult>> Login(
        [FromBody] CredentialsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(new LoginInput(request.Email, request.Password), cancellationToken);
        return Ok(result);
    }

    public sealed record CredentialsRequest(
        [Required, EmailAddress, StringLength(254)] string Email,
        [Required, MinLength(12), StringLength(128)] string Password);
}
