using System;
using Android.App;
using Android.Widget;
using Android.OS;
using HttpClientTest_CoreLib;
using ModernHttpClient;

namespace HttpClientTest_Android
{
	[Activity(Label = "HttpClientTest_Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class Activity1 : Activity
	{

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			Button btn1 = FindViewById<Button>(Resource.Id.btn1);
			Button btn2 = FindViewById<Button>(Resource.Id.btn2);

			btn1.Click += async (sender, args) =>
			{
				var shared = SharedClient.CreateWithMessageHandler(null);
				var s = await shared.GetData();
				Console.WriteLine("Received:");
				Console.WriteLine(s);
			};

			btn2.Click += async (sender, args) =>
			{
				var shared = SharedClient.CreateWithMessageHandler(new OkHttpNetworkHandler());
				var s = await shared.GetData();
				Console.WriteLine("Received:");
				Console.WriteLine(s);
			};
		}
	}
}

