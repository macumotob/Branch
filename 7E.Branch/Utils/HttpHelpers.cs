namespace _7E.Branch.Utils;


public static class HttpHelpers
{
    public static string? GetClientIp(HttpContext ctx)
    {
        var fwd = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(fwd)) return fwd.Split(',').FirstOrDefault()?.Trim();
        return ctx.Connection.RemoteIpAddress?.ToString();
    }
}