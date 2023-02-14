# Enum.Assert  
Automatic Create Enum unit test with Refelection and XUnit in .net core/C# , Creative Code With ErshadRaoufi

overLoad Of client api : 

<pre>static void WriteUnitTest(string path, string[] selectedAssembly)</pre>

<pre>static void WriteUnitTest(IDictionary<string, string> selectedAssembly)</pre>

overLoad Of ServicesExtensions: 

<pre>static void WriteUnitTest(this IServiceCollection services, string path, string[] selectedAssembly)</pre>

<pre>static void WriteUnitTest(this IServiceCollection services, IDictionary<string, string> selectedAssembly)</pre>

