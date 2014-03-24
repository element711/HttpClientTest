using System;
using System.Drawing;
using System.Net.Http;
using HttpClientTest_CoreLib;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace HttpClientTest_iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow(UIScreen.MainScreen.Bounds);

			var btn1 = new UIButton(UIButtonType.System)
			{
				Frame = new RectangleF(40, 50, 240, 40)
			};
			btn1.SetTitle("Test without CFNetworkHandler()", UIControlState.Normal);
			btn1.TouchUpInside += async (sender, args) =>
			{
				var shared = SharedClient.CreateWithMessageHandler(null);
				var s = await shared.GetData();
				Console.WriteLine("Received:");
				Console.WriteLine(s);

				var alert = new UIAlertView("Received", s, null, "OK", null);
				alert.Show();
			};


			var btn2 = new UIButton(UIButtonType.System)
			{
				Frame = new RectangleF(40, btn1.Frame.Bottom + 50, 240, 40)
			};
			btn2.SetTitle("Test with CFNetworkHandler()", UIControlState.Normal);
			btn2.TouchUpInside += async (sender, args) =>
			{
				var shared = SharedClient.CreateWithMessageHandler(new CFNetworkHandler());
				var s = await shared.GetData();
				Console.WriteLine("Received:");
				Console.WriteLine(s);
				var alert = new UIAlertView("Received", s, null, "OK", null);
				alert.Show();
			};

			var btn3 = new UIButton(UIButtonType.System)
			{
				Frame = new RectangleF(40, btn2.Frame.Bottom + 50, 240, 40)
			};
			btn3.SetTitle("Test with own HttpClient instance", UIControlState.Normal);
			btn3.TouchUpInside += async (sender, args) =>
			{
				var shared = SharedClient.CreateWithHttpClient(new HttpClient(new CFNetworkHandler()));
				var s = await shared.GetData();
				Console.WriteLine("Received:");
				Console.WriteLine(s);
				var alert = new UIAlertView("Received", s, null, "OK", null);
				alert.Show();
			};

			window.AddSubviews(btn1, btn2, btn3);

			// make the window visible
			window.MakeKeyAndVisible();

			return true;
		}
	}
}