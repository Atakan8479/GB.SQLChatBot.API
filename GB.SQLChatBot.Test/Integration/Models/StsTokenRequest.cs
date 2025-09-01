namespace GB.SQLChatBot.Test.Integration.Models;

public class StsTokenRequest
{
    public string? BaseAddress { get; set; }
    public string? ClientId { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? GrantType { get; set; }
    public string? ClientSecret { get; set; }
    public string? Scope { get; set; }
}