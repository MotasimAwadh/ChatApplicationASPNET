namespace ChatApplication.Models.ViewModels.MessagesViewModels
{
    public class ChatVM
    {
        public ChatVM()
        {
            Messages = new List<UserMessagesListVM>();
        }

        public string? CurrentUserId { get; set; }
        public string? ReciverId { get; set; }
        public string? ReciverUserName { get; set; }
        public IEnumerable<UserMessagesListVM> Messages { get; set; }
    }
}
