
namespace TestCreator.Core
{
    public static class EnumTest
    {
        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="path">path of unitTest creation</param>
        /// <param name="selectedAssembly">List Of Ass that you want Find Enums and Write Unit tests</param>
        /// <exception cref="ArgumentException">when Entry data is invalid</exception>
        public static void CreateUnitTestFilesFromAssemblies(string path, string[] selectedAssembly)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path is null");
            
            if (!Directory.Exists(path))
                throw new ArgumentException("path is not valid");

            if (!selectedAssembly.Any())
                throw new ArgumentException("selectedAssemly not given");

            new BaseUnitTestWriter().CreateUnitTestFile(path, selectedAssembly);
        }

        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="selectedAssembly">key : Assembly Name , Value : path of unitTest creation</param>
        public static void CreateUnitTestFilesFromAssemblies(IDictionary<string, string> selectedAssembly)
        {
            foreach (var assembly in selectedAssembly)
            {
                CreateUnitTestFilesFromAssemblies(assembly.Value, new[] { assembly.Key });
            }
        }
    }
}