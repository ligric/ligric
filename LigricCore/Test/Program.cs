using AbstractionBitZlatoRequests;
using AbstractionBoardRepository;
using AbstractionBoardRepository.Interfaces;
using BoardRepository;
using BoardRepository.BitZlato.API;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            

            string apiKey = "{" +
                            "\"kty\":\"EC\"," +
                            "\"alg\":\"ES256\"," +
                            "\"crv\":\"P-256\"," +
                            "\"x\":\"WnVJnRzpTUo0mYEdkiDSuyGqfDZtBVLepkzqHk7O8SE\"," +
                            "\"y\":\"J9P-SkGy4qyyL6f-T9KHtJzwiTASHcxAxmwtWiUVF1Q\"," +
                            "\"d\":\"1xF-MartEnw4cAQB3eJC-Eg5YwThMemMx96DuHhyGFA\"" +
                            "}";
            string email = "balalay16@gmail.com";

            IDictionary<string, string> filters = new Dictionary<string, string>();
            filters.Add("limit", "5");
            filters.Add("currency", "RUB");


            IBitZlatoRequestsService bitZlatoRequests = new BitZlatoRequests(apiKey, email);
            IAdBoardRepositoryWIthTimer adBoardRepository = new BoardBitZlatoRepository(filters, TimeSpan.FromSeconds(5), RepositoryStateEnum.Active, "BitZlato: currency -- RUB", bitZlatoRequests);

            adBoardRepository.AdsChanged += AdBoardRepository_AdsChanged;

            Console.ReadLine();
        }

        private static void AdBoardRepository_AdsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, Common.DtoTypes.Board.AdReadOnlyStruct> action)
        {
            Console.Clear();
           
        }
    }
}
