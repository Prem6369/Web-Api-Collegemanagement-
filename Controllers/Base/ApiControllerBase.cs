using Collegemanagement.extension;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Collegemanagement.Controllers.Base
{
    public class ApiControllerBase : ControllerBase
    {

        protected IActionResult ApiResponse<T>(ApiResponse<T> response)
        {
            return StatusCode(response.StatusCode, response);
        }

        protected IActionResult ApiOkResponse<T>(T data)
        {
            return ApiResponse(new ApiResponse<T>((int)HttpStatusCode.OK, "Success", data, null));
        }

        protected IActionResult ApiInsertResponse(int id)
        {
            return ApiResponse(new ApiResponse<int>((int)HttpStatusCode.OK, "Success", id, null));
        }

        //protected IActionResult ApiErrorResponse(Exception ex)
        //{
        //    var errorResponse = ex.CreateErrorResponse();
        //    return ApiResponse(new ApiResponse<object>(errorResponse.StatusCode, errorResponse.Message, null, errorResponse.Error));
        //}

        //protected IActionResult ApiResponse(ApiResponse response)
        //{
        //	return response.GetActionResult();
        //}

        //protected IActionResult ApiOkResponse()
        //{
        //	return ApiResponse(new ApiResponse((int)HttpStatusCode.OK));
        //}

        //protected IActionResult ApiOkResponse<TResult>(TResult response)
        //{
        //	return ApiResponse(new ApiOkResponse<TResult>(response));
        //}

        //protected IActionResult ApiInsertResponse(int id)
        //{
        //	return ApiResponse(new ApiInsertResponse(id));
        //}
    }
}
