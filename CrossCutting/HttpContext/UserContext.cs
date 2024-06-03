using System.IdentityModel.Tokens.Jwt;

namespace api.CrossCutting.HttpContext;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public string GetToken()
    {
        var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers.Authorization;

        return authorizationHeader.Any() is false ? string.Empty : authorizationHeader.ToString()[7..];
    }

    public Dictionary<string, string> GetHeaders()
        => _httpContextAccessor.HttpContext!.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

    public Dictionary<string, string> GetSpecificHeaders(params string[] headerKeys)
    {
        var headers = _httpContextAccessor.HttpContext!.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
        return headers.Where(h => headerKeys.Contains(h.Key)).ToDictionary(h => h.Key, h => h.Value);
    }

    public string? GetUserNameFromToken()
    {
        var token = GetToken();

        var jsonToken = new JwtSecurityTokenHandler().ReadToken(token);
        var tokenS = jsonToken as JwtSecurityToken;

        var name = tokenS?.Claims.First(x => x.Type == "name").Value;

        return name;
    }

    public string? GetEmailFromToken()
    {
        var token = GetToken();

        var jsonToken = new JwtSecurityTokenHandler().ReadToken(token);
        var tokenS = jsonToken as JwtSecurityToken;

        var email = tokenS?.Claims.First(x => x.Type == "email").Value;

        return email;
    }
}