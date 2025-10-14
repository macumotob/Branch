using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace _7E.Branch.Models;

public class Result : IActionResult
{
    public static bool UseLogger = true;
    public bool success { get; set; }
    public object? data { get; set; }
    public Result()
    {

    }

    public Result(bool success, object data)
    {
        this.success = success;
        this.data = data;
    }
    public static Result Error(string msg) => new Result
    {
        success = false,
        data = msg
    };
    public static Result Error(Exception ex)
    {
        return new Result
        {
            success = false,
            data = ex.Message
        };
    }
    public static Result Success(object data) => new Result
    {
        data = data,
        success = true,
    };
    public static Result Success()
    {
        return new Result
        {
            success = true,
            data = null
        };
    }
    public T? Get<T>() where T : class
    {
        return data as T;
    }
    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.ContentType = "application/json";
        response.StatusCode = success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
        var resultObj = new { success, message = data };
        var json = JsonSerializer.Serialize(resultObj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        await response.WriteAsync(json);
    }

}

