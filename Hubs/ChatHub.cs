using ChatApplication.Data;
using ChatApplication.Messages.Helpers;
using ChatApplication.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUserService _currentUserService;

        public ChatHub(ApplicationDbContext db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task SendMessage(string reciverId, string message)
        {
            var nowDate = DateTime.Now;
            var date = nowDate.ToShortDateString();
            var time = nowDate.ToShortTimeString();

            var senderId = _currentUserService.UserId;

            var messageToAdd = new Message()
            {
                Text = message,
                Date = nowDate,
                SenderId = senderId,
                ReceiverId = reciverId
            };

            await _db.Messages.AddAsync(messageToAdd);
            await _db.SaveChangesAsync();

            var users = new List<string>()
            {
                reciverId,
                senderId
            };

            await Clients.Users(users).SendAsync("ReceiveMessage", message, date, time, senderId);
        }
    }
}
