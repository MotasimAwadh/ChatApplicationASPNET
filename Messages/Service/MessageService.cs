using ChatApplication.Data;
using ChatApplication.Messages.Helpers;
using ChatApplication.Messages.Interface;
using ChatApplication.Models.ViewModels.MessagesViewModels;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Messages.Service
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUserService _currentUserService;
        public MessageService(ApplicationDbContext db, ICurrentUserService currentUserService) 
        {
            _db = db;
            _currentUserService = currentUserService;
        }
        public async Task<ChatVM> GetMessages(string selectedUserId)
        {
            var currentUserId = _currentUserService.UserId;

            var selectedUser = await _db.Users.FirstOrDefaultAsync(u => u.Id == selectedUserId);
            var selectedUserName = string.Empty;
            if (selectedUser is not null)
            {
                selectedUserName = selectedUser.UserName;
            }

            var chatVM = new ChatVM()
            {
                CurrentUserId = currentUserId,
                ReciverId = selectedUserId,
                ReciverUserName = selectedUserName,
            };

            var messages = await _db.Messages
             .Where(
             i => (i.SenderId == currentUserId && i.ReceiverId == selectedUserId) ||
                  (i.SenderId == selectedUserId && i.ReceiverId == currentUserId))
             .Select(i => new UserMessagesListVM()
             {
                 Id = i.Id,
                 Text = i.Text,
                 Date = i.Date.ToShortDateString(),
                 Time = i.Date.ToShortTimeString(),
                 IsCurrentUserSentMessage = i.SenderId == currentUserId,
             }).ToListAsync();

            chatVM.Messages = messages;
            return chatVM;
        }

        public async Task<IEnumerable<MessagesUsersListVM>> GetUsers()
        {
            var currentUserId = _currentUserService.UserId;

            var users = await _db.Users.Where(i => i.Id != currentUserId).Select(i => new MessagesUsersListVM()
            {
                Id = i.Id,
                UserName = i.UserName,
                LastMessage = _db.Messages.Where(m => (m.SenderId == currentUserId || m.SenderId == i.Id) && (m.ReceiverId == currentUserId || m.ReceiverId == i.Id)).OrderByDescending(m => m.Id).Select(m => m.Text).FirstOrDefault(),
            }).ToListAsync();

            return users;
        }
    }
}
