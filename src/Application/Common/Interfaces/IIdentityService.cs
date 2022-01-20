using ProductsArchive.Application.Common.Models;

namespace ProductsArchive.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserRoleAsync(string userId);

    Task<bool> UserExistsAsync(string userId);

    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
}
