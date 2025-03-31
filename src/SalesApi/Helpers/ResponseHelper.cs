using Microsoft.AspNetCore.Mvc;

public static class ResponseHelper
{
    public static IActionResult SuccessResponse(object data, string message = "Operação concluída com sucesso")
    {
        return new OkObjectResult(new ApiResponse
        {
            Data = data,
            Status = "success",
            Message = message
        });
    }
}
