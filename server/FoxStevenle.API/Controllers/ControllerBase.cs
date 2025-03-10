using System.Diagnostics;
using FoxStevenle.API.Types.OptionalResult;
using Microsoft.AspNetCore.Mvc;

namespace FoxStevenle.API.Controllers;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
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