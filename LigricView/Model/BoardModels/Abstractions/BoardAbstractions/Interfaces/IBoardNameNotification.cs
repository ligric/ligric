namespace BoardsShared.Abstractions.BoardAbstractions.Interfaces
{
    public delegate void ActionNameHandler(object sender, string name);

    public interface IBoardNameNotification
    {
        string Name { get; }

        bool SetNameAndSendAction(string name);

        event ActionNameHandler NameChanged;
    }
}
