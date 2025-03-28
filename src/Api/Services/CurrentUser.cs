using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Api.Services;

/// <summary>
///     Current user service
/// </summary>
/// <param name="httpContextAccessor"></param>
public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    /// <summary>
    /// </summary>
    public string? Id
    {
        get
        {
            // try get X-Authenticated-Userid 
            if (httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("X-Authenticated-Userid",
                    out var userId) == true)
                return userId;

            // try get oid from claims
            return httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
        }
    }

    /// <summary>
    /// </summary>
    public string? UserName
    {
        get
        {
            if (httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("X-Authenticated-Username",
                    out var userName) == true)
                return userName;

            return httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.Name);
        }
    }
}