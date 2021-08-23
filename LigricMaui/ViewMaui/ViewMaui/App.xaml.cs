using AbstractionBitZlatoRequests;
using AbstractionBoardRepository;
using AbstractionBoardRepository.Interfaces;
using BoardRepository;
using BoardRepository.BitZlato.API;
using Common.DtoTypes.Board;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;

namespace ViewMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

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
            IAdBoardRepositoryWIthTimer adBoardRepository = new BoardBitZlatoRepository(filters, TimeSpan.FromSeconds(5), RepositoryStateEnum.Stoped, "BitZlato: currency -- RUB", bitZlatoRequests);

            adBoardRepository.AdsChanged += AdBoardRepository_AdsChanged;
        }

        private void AdBoardRepository_AdsChanged(object sender, NotifyDictionaryChangedEventArgs<long, AdReadOnlyStruct> e)
        {

        }
    }
}
