using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientTest_CoreLib
{
    public class SharedClient
    {
		private SharedClient()
		{ }


		public static SharedClient CreateWithHttpClient(HttpClient c)
		{
			var shared = new SharedClient
			{
				client = c
			};
			return shared;
		}

		public static SharedClient CreateWithMessageHandler(HttpMessageHandler handler)
		{
			var shared = new SharedClient
			{
				client = handler == null ? new HttpClient() : new HttpClient(handler, false)
			};
			return shared;
		}

		HttpClient client;

		public async Task<string> GetData()
		{
			string result = string.Empty;
			result = await this.client.GetStringAsync("http://rxnav.nlm.nih.gov/REST/drugs?name=aspirin");
			return result;
		}
    }
}
