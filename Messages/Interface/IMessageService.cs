using ChatApplication.Models.ViewModels.MessagesViewModels;

namespace ChatApplication.Messages.Interface
{
    public interface IMessageService
    {
        Task<IEnumerable<MessagesUsersListVM>> GetUsers();
        Task<ChatVM> GetMessages(string selectedUserId);
    }
}
