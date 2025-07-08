namespace PersonalFinanceTracker.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password);
        Task<bool> RegisterUserAsync(string Email, string username, string password);
    }
}
