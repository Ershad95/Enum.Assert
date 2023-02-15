namespace TestCreator.Core
{
    public static class EnumTest
    {
        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="path">path of unitTest creation</param>
        /// <param name="selectedAssembly">List Of Ass that you want Find Enums and Write Unit tests</param>
        /// <param name="assertType">Select Assert Type</param>
        /// <param name="unitTestFrameworkType">Select unitTest Framework</param>
        /// <exception cref="ArgumentException">when Entry data is invalid</exception>
        public static void CreateUnitTestFilesFromAssemblies(string path, string[] selectedAssembly,
            AssertType assertType,
            UnitTestFrameworkType unitTestFrameworkType)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path is null");

            if (!Directory.Exists(path))
                throw new ArgumentException("path is not valid");

            if (!selectedAssembly.Any())
                throw new ArgumentException("selectedAssemly not given");

            var baseUnitTestWriter = new DefaultUnitTestWriter();
            baseUnitTestWriter.SetAssertType(assertType);
            baseUnitTestWriter.SetUnitTestFrameworkType(unitTestFrameworkType);
            baseUnitTestWriter.CreateUnitTestFile(path, selectedAssembly);
        }
        
        public static void CreateUnitTestFilesFromAssemblies(
            string path, 
            string[] selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path is null");

            if (!Directory.Exists(path))
                throw new ArgumentException("path is not valid");

            if (!selectedAssembly.Any())
                throw new ArgumentException("selectedAssemly not given");
            
            baseUnitTestWriter.CreateUnitTestFile(path, selectedAssembly);
        }

        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="selectedAssembly">key : Assembly Name , Value : path of unitTest creation</param>
        /// <param name="assertType">Select Assert Type</param>
        /// <param name="unitTestFrameworkType">Select unitTest Framework</param>
        public static void CreateUnitTestFilesFromAssemblies(IDictionary<string, string> selectedAssembly,
            AssertType assertType,
            UnitTestFrameworkType unitTestFrameworkType)
        {
            foreach (var assembly in selectedAssembly)
            {
                CreateUnitTestFilesFromAssemblies(assembly.Value, new[] { assembly.Key }, assertType, unitTestFrameworkType);
            }
        }
        
        public static void CreateUnitTestFilesFromAssemblies(IDictionary<string, string> selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter)
        {
            foreach (var assembly in selectedAssembly)
            {
                CreateUnitTestFilesFromAssemblies(assembly.Value, new[] { assembly.Key }, baseUnitTestWriter);
            }
        }
    }
}