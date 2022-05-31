using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pi_Books.Data.ViewModels;

namespace Pi_Books.ActionResults
{
    public class CustomActionResult : IActionResult
    {
        private readonly CustomActionResultVM customActionResult;

        public CustomActionResult(CustomActionResultVM customActionResult)
        {
            this.customActionResult = customActionResult;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(customActionResult.Exception ?? customActionResult.Publisher as object)
            {
                StatusCode=customActionResult.Exception!=null?StatusCodes.Status500InternalServerError:StatusCodes.Status200OK
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}