using ChatApplication.Models;

namespace ChatApplication.Messages.Helpers
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        Task<AppUser> GetUser();
    }
}
