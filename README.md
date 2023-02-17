# Enum.Assert  

## Project Description
Automatic Create Enum unit test with Refelection and XUnit in .net core/C# , Creative Code With ErshadRaoufi

### Overview
##### overLoad Of client api : 

<pre>static void CreateUnitTestFilesFromAssemblies(
            string path,
            string[] selectedAssembly,
            AssertType assertType = AssertType.Assert,
            UnitTestFrameworkType unitTestFrameworkType = UnitTestFrameworkType.XUnit,
            bool overWriteTests = false)</pre>

<pre>static void CreateUnitTestFilesFromAssemblies(
            string path,
            string[] selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter,
            bool overWriteTests = false)</pre>
            
<pre>static void CreateUnitTestFilesFromAssemblies(
            IDictionary<string, string> selectedAssembly,
            AssertType assertType = AssertType.Assert,
            UnitTestFrameworkType unitTestFrameworkType = UnitTestFrameworkType.XUnit,
            bool overWriteTests = false))</pre>
            
<pre>static void CreateUnitTestFilesFromAssemblies(
            IDictionary<string, string> selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter,
            bool overWriteTests = false)</pre>

##### overLoad Of Extensions for IServiceCollection: 

<pre>static void WriteUnitTest(this IServiceCollection services,
            string path,
            string[] selectedAssembly,
            AssertType assertType,
            UnitTestFrameworkType unitTestFrameworkType,
            bool overWriteTests = false)</pre>
            
<pre>static void WriteUnitTest(this IServiceCollection services,
            IDictionary<string, string> selectedAssembly,
            AssertType assertType,
            UnitTestFrameworkType unitTestFrameworkType,
            bool overWriteTests = false)</pre>
            
<pre>static void WriteUnitTest(this IServiceCollection services,
            string path,
            string[] selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter,
            bool overWriteTests = false)</pre>

<pre>static void WriteUnitTest(this IServiceCollection services,
            IDictionary<string, string> selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter,
            bool overWriteTests = false)</pre>
