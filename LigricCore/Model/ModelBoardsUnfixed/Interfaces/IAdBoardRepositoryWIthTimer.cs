namespace AbstractionBoardRepository.Interfaces
{
    public interface IAdBoardRepositoryWIthTimer : IAdBoardRepository
    {
        bool SetUpdateTime(TimeSpan time);
        bool StartRepository();
        bool StopRepository();
    }
}
