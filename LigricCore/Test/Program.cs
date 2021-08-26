using AbstractionBitZlatoRequests;
using AbstractionBoardRepository;
using AbstractionBoardRepository.Interfaces;
using BoardRepository;
using BoardRepository.BitZlato.API;
using Common.EventArgs;
using System.Linq;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {

        public static IAdBoardRepositoryWIthTimer adBoardRepository;
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
            filters.Add("limit", "20");
            filters.Add("currency", "RUB");


            IBitZlatoRequestsService bitZlatoRequests = new BitZlatoRequests(apiKey, email);
            adBoardRepository = new BoardBitZlatoRepository(filters, TimeSpan.FromSeconds(5), RepositoryStateEnum.Active, "BitZlato: currency -- RUB", bitZlatoRequests);

            adBoardRepository.AdsChanged += AdBoardRepository_AdsChanged;

            Console.ReadLine();
            
        }

        private static void AdBoardRepository_AdsChanged(object? sender, NotifyDictionaryChangedEventArgs<long, Common.DtoTypes.Board.AdDto> e)
        {
            Console.Clear();


            foreach (var ad in adBoardRepository.Ads)
            {
               

                Console.WriteLine("Id\t" + ad.Value.Id + 
                                  "\nType\t" + ad.Value.Type.ToString() + 
                                  "\nCrypto currency\t" + ad.Value.Rate.RightCurrency +
                                  "\nCurrency\t" + ad.Value.Rate.LeftCurrency + "\nRate\t" + ad.Value.Rate.Value +
                                  "\nLimit Currency\n" + 
                                        "Min\t" + ad.Value.LimitCurrencyLeft.From + 
                                        "\nMax\t" + ad.Value.LimitCurrencyLeft.To + 
                                        "\nReal max\t" + ad.Value.LimitCurrencyLeft.RealMax +
                                  "\nLimit Cryptocurrency\n" + 
                                        "Min\t" + ad.Value.LimitCurrencyRight.From + 
                                        "\nMax\t" + ad.Value.LimitCurrencyRight.To + 
                                        "\nReal max\t" + ad.Value.LimitCurrencyRight.RealMax +
                                  "\nPaymethod" +
                                        "\n\tId\t" + ad.Value.Paymethod.Id +
                                        "\n\tName\t" + ad.Value.Paymethod.Name +
                                  "\nPaymethod id\t" + ad.Value.Paymethod.Id +
                                  "\nOwner\t" + ad.Value.Trader.Name +
                                  "\nOwner last activity\t" + ad.Value.Trader.LastActivity +
                                  "\nIs owner verificated\t" + ad.Value.Trader.Verificated +
                                  "\nSafe mode\t" + ad.Value.SafeMode +
                                  "\nOwner trusted\t" + ad.Value.Trader.Trusted +
                                  "\nOwner balance\t" + ad.Value.Trader.Balance);
            }
           
        }
    }
}
