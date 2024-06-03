namespace api.CrossCutting.HttpContext;

public interface IUserContext
{
    string GetToken();
    Dictionary<string, string> GetHeaders();
    Dictionary<string, string> GetSpecificHeaders(params string[] headerKeys);
    string? GetUserNameFromToken();
    string? GetEmailFromToken();
}