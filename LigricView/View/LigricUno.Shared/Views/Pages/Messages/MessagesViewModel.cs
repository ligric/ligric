using System;
using System.Collections.ObjectModel;

namespace LigricUno.Views.Pages.Messages
{
    public class MessagesViewModel
    {
        public ObservableCollection<MessageItem> Messages { get; } = new ObservableCollection<MessageItem>()
        {
            new MessageItem("ms-appx:///Assets/Images/Alexey True.jpg", "Alexey True", "😁😁😁", DateTime.Now),
            new MessageItem(String.Empty, "Célia B", "I’m just really disappointed that you do not share anything because that’s what friends are supposed to do if you trust each other. But you make me feel like a stranger to you. So let’s keep it this way, I wish you the best for the future, keep the good work and keep it all for you, I won’t support you anymore and you never wanted my support in the first place so it doesn’t matter. Goodbye",
                DateTime.Now),
            new MessageItem("ms-appx:///Assets/Images/Gogo.jpg", "Gogo 郭", "ty", DateTime.Now),
            new MessageItem("ms-appx:///Assets/Images/elen.jpg", "elen.", "😸", DateTime.Now),
            new MessageItem(String.Empty, "Trip Ray", "🙏🙏🙏", DateTime.Now),
            new MessageItem("ms-appx:///Assets/Images/Даня Караконстантин.jpg", "Даня Караконстантин", "ыыыы", DateTime.Now, 3),
            new MessageItem("ms-appx:///Assets/Images/Игорёк.jpg", "Игорёк", "угу", DateTime.Now),
            new MessageItem("ms-appx:///Assets/Images/Алексей Шевцов.jpg", "Алексей Шевцов", "Какое же лютое мракобесие...", DateTime.Now, 9)
        };
    }

    public class MessageItem
    {
        public string ImageUri { get; }

        public string Name { get; }

        public string LastMessage { get; }

        public DateTime LastMessageTime { get; }

        public long UnreadedMessagesCount { get; }


        public MessageItem(string imageUri, string name, string lastMessage, DateTime lastMessageTime, long unreadedMessagesCount = 0)
        {
            ImageUri = imageUri;
            Name = name;
            LastMessage = lastMessage;
            LastMessageTime = lastMessageTime;
            UnreadedMessagesCount = unreadedMessagesCount;
        }
    }
}
