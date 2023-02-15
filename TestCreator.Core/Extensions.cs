using Microsoft.Extensions.DependencyInjection;

namespace TestCreator.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="services"></param>
        /// <param name="path">path of unitTest creation</param>
        /// <param name="selectedAssembly">List Of Ass that you want Find Enums and Write Unit tests</param>
        /// <param name="assertType"></param>
        /// <param name="unitTestFrameworkType"></param>
        /// <exception cref="ArgumentException">when Entry data is invalid</exception>
        public static void WriteUnitTest(this IServiceCollection services,
            string path,
            string[] selectedAssembly,
            AssertType assertType,
            UnitTestFrameworkType unitTestFrameworkType)
        {
            EnumTest.CreateUnitTestFilesFromAssemblies(path, selectedAssembly, assertType, unitTestFrameworkType);
        }

        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="services"></param>
        /// <param name="selectedAssembly">key : Assembly Name , Value : path of unitTest creation</param>
        /// <param name="assertType"></param>
        /// <param name="unitTestFrameworkType"></param>
        public static void WriteUnitTest(this IServiceCollection services,
            IDictionary<string, string> selectedAssembly,
            AssertType assertType,
            UnitTestFrameworkType unitTestFrameworkType)
        {
            EnumTest.CreateUnitTestFilesFromAssemblies(selectedAssembly, assertType, unitTestFrameworkType);
        }


        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="services"></param>
        /// <param name="path">path of unitTest creation</param>
        /// <param name="selectedAssembly">List Of Ass that you want Find Enums and Write Unit tests</param>
        /// <param name="baseUnitTestWriter"></param>
        /// <exception cref="ArgumentException">when Entry data is invalid</exception>
        public static void WriteUnitTest(this IServiceCollection services,
            string path,
            string[] selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter)
        {
            EnumTest.CreateUnitTestFilesFromAssemblies(path, selectedAssembly, baseUnitTestWriter);
        }

        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="services"></param>
        /// <param name="selectedAssembly">key : Assembly Name , Value : path of unitTest creation</param>
        /// <param name="baseUnitTestWriter"></param>
        public static void WriteUnitTest(this IServiceCollection services,
            IDictionary<string, string> selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter)
        {
            EnumTest.CreateUnitTestFilesFromAssemblies(selectedAssembly, baseUnitTestWriter);
        }
    }
}