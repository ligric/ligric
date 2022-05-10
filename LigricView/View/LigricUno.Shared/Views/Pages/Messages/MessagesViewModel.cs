using System.Collections.ObjectModel;

namespace LigricUno.Views.Pages.Messages
{
    public class MessagesViewModel
    {
        public ObservableCollection<MessageItem> Messages { get; } = new ObservableCollection<MessageItem>()
        {
            new MessageItem("", "Иван", "Го бухать"),
            new MessageItem("", "Иван", "Го бухать"),
            new MessageItem("", "Иван", "Го бухать"),
            new MessageItem("", "Иван", "Го бухать"),
            new MessageItem("", "Иван", "Го бухать"),
            new MessageItem("", "Иван", "Го бухать")
        };
    }

    public class MessageItem
    {
        public string ImageUri { get; }
        public string Name { get; }
        public string LastMessage { get; }

        public MessageItem(string imageUri, string name, string lastMessage)
        {
            ImageUri = imageUri;
            Name = name;
            LastMessage = lastMessage;
        }
    }
}
