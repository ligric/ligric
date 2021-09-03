using BitZlatoApi;

namespace BoardRepository.BitZlato
{
    public partial class BitZlatoWithTimerRepository
    {
        public BitZlatoWithTimerRepository(string apiKey, string email, RepositoryStateEnum repositoryState = RepositoryStateEnum.Stoped)
            : base(repositoryState)
        {
            bitZlatoApi = new BitZlatoRequests(apiKey, email);
        }

        public BitZlatoWithTimerRepository(string apiKey, string email, TimeSpan timerInterval, RepositoryStateEnum repositoryState = RepositoryStateEnum.Stoped)
            : base(timerInterval, repositoryState)
        {
            bitZlatoApi = new BitZlatoRequests(apiKey, email);
        }

        public BitZlatoWithTimerRepository(string apiKey, string email, TimeSpan timerInterval, Dictionary<string, string> parametrs, RepositoryStateEnum repositoryState = RepositoryStateEnum.Stoped)
            : base(timerInterval, repositoryState)
        {
            this.parametrs = parametrs;
            bitZlatoApi = new BitZlatoRequests(apiKey, email);
        }
    }
}
