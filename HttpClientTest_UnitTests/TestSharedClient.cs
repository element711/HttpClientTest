using System.Net.Http;
using HttpClientTest_CoreLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace HttpClientTest_UnitTests
{
	[TestClass]
	public class TestSharedClient
	{
		[TestMethod]
		public async Task TestGetData()
		{
			var shared = SharedClient.CreateWithMessageHandler(null);
			string s = await shared.GetData();
			Assert.IsNotNull(s, "Return value is NULL!");
			Assert.IsTrue(s.Length > 0, "No data returned!");
		}
	}
}
