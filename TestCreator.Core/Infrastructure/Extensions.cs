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
        /// <param name="overWriteTests"></param>
        /// <exception cref="ArgumentException">when Entry data is invalid</exception>
        public static void WriteUnitTest(this IServiceCollection services,
            string path,
            string[] selectedAssembly,
            AssertType assertType,
            UnitTestFrameworkType unitTestFrameworkType,
            bool overWriteTests = false)
        {
            TestWriter.CreateUnitTestFilesFromAssemblies(path,
                selectedAssembly, assertType,
                unitTestFrameworkType,
                overWriteTests);
        }

        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="services"></param>
        /// <param name="selectedAssembly">key : Assembly Name , Value : path of unitTest creation</param>
        /// <param name="assertType"></param>
        /// <param name="unitTestFrameworkType"></param>
        /// <param name="overWriteTests"></param>
        public static void WriteUnitTest(this IServiceCollection services,
            IDictionary<string, string> selectedAssembly,
            AssertType assertType,
            UnitTestFrameworkType unitTestFrameworkType,
            bool overWriteTests = false)
        {
            TestWriter.CreateUnitTestFilesFromAssemblies(
                selectedAssembly,
                assertType,
                unitTestFrameworkType,
                overWriteTests);
        }


        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="services"></param>
        /// <param name="path">path of unitTest creation</param>
        /// <param name="selectedAssembly">List Of Ass that you want Find Enums and Write Unit tests</param>
        /// <param name="baseUnitTestWriter"></param>
        /// <param name="overWriteTests"></param>
        /// <exception cref="ArgumentException">when Entry data is invalid</exception>
        public static void WriteUnitTest(this IServiceCollection services,
            string path,
            string[] selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter,
            bool overWriteTests = false)
        {
            TestWriter.CreateUnitTestFilesFromAssemblies(
                path,
                selectedAssembly,
                baseUnitTestWriter,
                overWriteTests);
        }

        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="services"></param>
        /// <param name="selectedAssembly">key : Assembly Name , Value : path of unitTest creation</param>
        /// <param name="baseUnitTestWriter"></param>
        /// <param name="overWriteTests"></param>
        public static void WriteUnitTest(this IServiceCollection services,
            IDictionary<string, string> selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter,
            bool overWriteTests = false)
        {
            TestWriter.CreateUnitTestFilesFromAssemblies(
                selectedAssembly,
                baseUnitTestWriter,
                overWriteTests);
        }
    }
}