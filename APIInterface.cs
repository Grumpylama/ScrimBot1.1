using System.Text;
/*namespace big
{


    public class APIInterface
    {
        public Uri BaseAddress { get; set; }
        public APIInterface(Uri BaseAddress)
        {
            this.BaseAddress = BaseAddress;
        }

        public static string getData()
        {
            return "Hello World";
        }

        public static async Task<HttpResponseMessage> SendRequestTestReqest()
        {
            var client = new HttpClient();
            var content = new StringContent("{'key':'value'}", Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:3001", content);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return response;
        }
        public static async Task<HttpResponseMessage> SendRequest(ServerRequest r)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://localhost:3001/api/Request", r);
            
            return response;
        }
    }
}
*/