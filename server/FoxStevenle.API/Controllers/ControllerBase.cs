using System.Diagnostics;
using FoxStevenle.API.Types.OptionalResult;
using Microsoft.AspNetCore.Mvc;

namespace FoxStevenle.API.Controllers;

public class ControllerBase(ILogger logger) : Microsoft.AspNetCore.Mvc.ControllerBase
{
    /// <summary>
    /// Creates a request response based on the passed <see cref="Optional{T}"/>
    /// </summary>
    /// <param name="optionalResponse"><see cref="Optional{T}"/> to base the response on</param>
    /// <typeparam name="T">Type of the optional response</typeparam>
    /// <returns>Request response as <see cref="IActionResult"/></returns>
    /// <exception cref="UnreachableException">Thrown if <see cref="OptionalErrorType"/> is <see cref="OptionalErrorType.Unknown"/> or not defined</exception>
    protected IActionResult CreateActionResultResponse<T>(Optional<T> optionalResponse)
    {
        if (optionalResponse.HasValue)
        {
            return Ok(optionalResponse.Value);
        }

        switch (optionalResponse.Error.Type)
        {
            case OptionalErrorType.NotFound:
                return NotFound(optionalResponse.GetErrorMessageWrapper());
            case OptionalErrorType.Forbidden:
                return StatusCode(StatusCodes.Status403Forbidden, optionalResponse.GetErrorMessageWrapper());
            case OptionalErrorType.BadRequest:
                return BadRequest(optionalResponse.GetErrorMessageWrapper());
            case OptionalErrorType.InternalServerError:
                logger.LogError("Internal server error occured: {Message}", optionalResponse.Error.Message);
#if DEBUG
                return StatusCode(StatusCodes.Status500InternalServerError, optionalResponse.GetErrorMessageWrapper());
#else
                return StatusCode(StatusCodes.Status500InternalServerError);
#endif
            case OptionalErrorType.Unknown:
            default:
                throw new UnreachableException("CreateActionResult reached unreachable code.");
        }
    }
}