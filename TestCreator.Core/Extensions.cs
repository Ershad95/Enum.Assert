using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TestCreator.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Create Unit Test For Enums
    /// </summary>
    /// <param name="services"></param>
    /// <param name="path">path of unitTest creation</param>
    /// <param name="selectedAssembly">List Of Ass that you want Find Enums and Write Unit tests</param>
    /// <exception cref="ArgumentException">when Entry data is invalid</exception>
    public static void WriteUnitTest(this IServiceCollection services, string path, string[] selectedAssembly)
    {
        EnumTest.CreateUnitTestFilesFromAssemblies(path, selectedAssembly);
    }

    /// <summary>
    /// Create Unit Test For Enums
    /// </summary>
    /// <param name="services"></param>
    /// <param name="selectedAssembly">key : Assembly Name , Value : path of unitTest creation</param>
    public static void WriteUnitTest(this IServiceCollection services, IDictionary<string, string> selectedAssembly)
    {
        EnumTest.CreateUnitTestFilesFromAssemblies(selectedAssembly);
    }
}

public static class AssemblyExtensions
{
    /// <summary>
    /// Create Unit Test For Enums
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="path">path of unitTest creation</param>
    /// <exception cref="ArgumentException">when Entry data is invalid</exception>
    public static void WriteUnitTest(this Assembly @assembly, string path)
    {
        EnumTest.CreateUnitTestFilesFromAssemblies(path,new []{@assembly.FullName!});
    }
}