using BoardRepository.BitZlato.API;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
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

            BitZlatoRequests requests = new BitZlatoRequests(apiKey, email);
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("limit", "5");
            parameters.Add("currency", "RUB");
            var res = requests.GetAdsFromFilters(parameters);
            Console.WriteLine("Hello, World!");
        }
    }
}
