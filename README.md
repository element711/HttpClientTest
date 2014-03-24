# HttpClient Test Solution

The intention of this solution is to demonstrate and hopefully (with the help of you) **solve** the issues many of us encounter when using HttpClient in combination with PCLs and Xamarin projects.

The solution has four projects:

* Android
* iOS
* Unit Test
* Shared library

## The shared library
HttpClientTest_CoreLib is a PCL targeting .NET 4.5, Windows 8 Store, Xamarin iOS, Xamarin Android and Windows Phone 8. In this combination, the namespace `System.Net.Http` is not available. To overcome this a Nuget package was added (https://www.nuget.org/packages/Microsoft.Net.Http/2.2.18). This replicates the namespace.

The shared library offers one class which allows initializing a client to retrieve some data from the web:

`SharedClient(HttpMessageHandler handler)`

The `handler` is from `System.Net.Http.HttpMessageHandler`.
The client can be initialized by passing a specific handler or NULL. When calling `GetData()` it will retrieve a string (async) from http://rxnav.nlm.nih.gov/REST/drugs?name=aspirin.

## The unit test project
No issues here. This works as expected. It verifies that data is actually retrieved successfully.

## The iOS project
It comes with three buttons. The first one passes a NULL handler to the shared client. Clicking that button retrieves data successfully. All is handled by 'Microsoft.Net.Http' as expected.

The second button however tries to pass an instance of 'CFNetworkHandler' which is defined in 'System.Net.Http' on iOS and is an implementation of an 'HttpMessageHandler' that uses special iOS specific API. It comes with Xamarin.iOS.

The third button tries to pass its own instance of `HttpClient`, however it fails with the same exception as button two. See below.

The solution builds fine, however at runtime when clicking the 2nd or 3rd button, an exception occurs:

````
{System.TypeLoadException: Could not load type 'System.Net.Http.CFNetworkHandler' from assembly 'HttpClientTest_iOS'.  at System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start[<<FinishedLaunching>b__3>d__b] 
````

The explanation is fairly straight forward: `CFNetworkHandler` is not part of the assembly from the Nuget package. There is actually a hint in the warning when building:

> Warning	3	Found conflicts between different versions of the same dependent assembly. Please set the "AutoGenerateBindingRedirects" property to true in the project file. For more information, see http://go.microsoft.com/> fwlink/?LinkId=294190.	HttpClientTest_iOS

However **I have no idea if this is the cause of the issue and how to fix it**.

Also note that I have actually added the recommended assembly redirects as stated by James Montemagno at http://motzcod.es/post/78863496592/portable-class-libraries-httpclient-so-happy:

````
<dependentAssembly>
	<assemblyIdentity name="System.Net.Http" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
	<bindingRedirect oldVersion="0.0.0.0-4.0.0.0" newVersion="2.0.5.0" />
</dependentAssembly>
````

## The Android project
On Android I do pretty much the same as on iOS. The first button  is not using any specific handlers and works.

The second one uses `ModernHttpClient` (https://github.com/paulcbetts/ModernHttpClient) and also succeeds.

This proves to me that the problem on iOS really seems to be coming from the namespaces.

# Conclusion and opens

* How can I get CFNetworkHandler to work across PCL boundaries?
* What is this build warning about?