# Enum.Assert  

## Project Description
This is a C# library called "Enum.Assert" that allows you to easily generate unit tests for your enum types. It contains a single public class called "TestWriter" with four static methods that can be used to generate unit tests for enums in different ways.

The namespace for the library is "TestCreator.Core". The library uses two enums called "AssertType" and "UnitTestFrameworkType" to specify the type of assertions to be used in the generated unit tests and the type of unit test framework to use, respectively.Creative Code With ErshadRaoufi

### Overview
##### The "TestWriter" class contains four static methods for generating unit tests:
 
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
