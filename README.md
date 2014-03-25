# HttpClient Test Solution

The intention of this solution is to demonstrate how to use `HttpClient` from a PCL from:

* Android
* iOS
* Unit Test

## The shared library
HttpClientTest_CoreLib is a PCL targeting .NET 4.5, Windows 8 Store, Xamarin iOS, Xamarin Android and Windows Phone 8. In this combination, the namespace `System.Net.Http` is not available. To overcome this a Nuget package was added (https://www.nuget.org/packages/Microsoft.Net.Http/2.2.18). This replicates the namespace in the assembly `Microsoft.Net.Http`.

## The unit test project
No issues here. This works as expected. It verifies that data is actually retrieved successfully.

## The iOS project

It demonstrates how to use the PCL and how to let `System.Net.Http` and `Microsoft.Net.Http` coexist in peace.

* It comes with three buttons. The first one passes a NULL handler to the shared client. Clicking that button retrieves data successfully. All is handled by 'Microsoft.Net.Http' as expected.
* The second button passes an instance of 'CFNetworkHandler' which is defined in 'System.Net.Http' on iOS and is an implementation of an 'HttpMessageHandler' that uses special iOS specific API. It comes with Xamarin.iOS.
* The third button passes its own instance of `HttpClient`.

Without changes to the app.config file the 2nd and 3rd button would not work. At runtime you would get an exception because it will try to us the `Microsoft.Net.Http` assembly which does not know anything about `CFNetworkHandler`:


````
{System.TypeLoadException: Could not load type 'System.Net.Http.CFNetworkHandler' from assembly 'HttpClientTest_iOS'.  at System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start[<<FinishedLaunching>b__3>d__b] 
````

The app.config needs to change and contain:

````
<dependentAssembly>
	<assemblyIdentity name="System.Net.Http" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
	<bindingRedirect oldVersion="0.0.0.0-4.0.0.0" newVersion="2.0.5.0" />
</dependentAssembly>
````

This will redirect all calls to the correct assembly.

In addtion packages.config required on entry too:

TODO: WHY IS THIS REQUIRED?

````
<package id="Microsoft.Bcl.Build" version="1.0.10" targetFramework="MonoTouch10" />
````

## The Android project

On Android I do pretty much the same as on iOS.

* The first button is not using any specific handlers and works.
* The second one uses `ModernHttpClient` (https://github.com/paulcbetts/ModernHttpClient) and also succeeds.

Android is working without any further changes.