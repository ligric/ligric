namespace AbstractionBoardRepository.Interfaces
{
    public delegate void ActionNameHandler(object sender, string name);
    public interface IAdBoardNameNotification
    {
        string Name { get; }

        bool SetNameAndSendAction(string name);

        event ActionNameHandler NameChanged;
    }
}
