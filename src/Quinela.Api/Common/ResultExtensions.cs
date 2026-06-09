using Quinela.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Quinela.Api.Common
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this Result result)
        {
            if (result.IsSuccess)
                return new OkResult();

            return BuildProblem(result.Error);
        }

        public static IActionResult ToNoContentResult(this Result result)
        {
            if (result.IsSuccess)
                return new NoContentResult();

            return BuildProblem(result.Error);
        }

        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Value);

            return BuildProblem(result.Error);
        }

        private static IActionResult BuildProblem(Error error)
        {
            var status = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status400BadRequest
            };

            return new ObjectResult(new { message = error.Message }) { StatusCode = status };
        }
    }
}
