namespace TestCreator.Core
{
    /// <summary>
    /// use this class for Call Public Api of Test creator or use public Extension Api
    /// </summary>
    public static class TestWriter
    {
        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="path">path of unitTest creation</param>
        /// <param name="selectedAssembly">List Of Ass that you want Find Enums and Write Unit tests</param>
        /// <param name="assertType">Select Assert Type</param>
        /// <param name="unitTestFrameworkType">Select unitTest Framework</param>
        /// <exception cref="ArgumentException">when Entry data is invalid</exception>
        public static void CreateUnitTestFilesFromAssemblies(
            string path,
            string[] selectedAssembly,
            AssertType assertType = AssertType.Assert,
            UnitTestFrameworkType unitTestFrameworkType = UnitTestFrameworkType.XUnit,
            bool overWriteTests = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path is null");

            if (!Directory.Exists(path))
                throw new ArgumentException("path is not valid");

            if (!selectedAssembly.Any())
                throw new ArgumentException("selectedAssemly not given");

            new DefaultUnitTestWriter()
                .SetTestAssertType(assertType)
                .SetTestFrameworkType(unitTestFrameworkType)
                .SetCreationPath(path)
                .SetAssemblies(selectedAssembly)
                .OverwriteTests(overWriteTests)
                .Write();
        }

        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="path"></param>
        /// <param name="selectedAssembly">List Of Ass that you want Find Enums and Write Unit tests</param>
        /// <param name="baseUnitTestWriter"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void CreateUnitTestFilesFromAssemblies(
            string path,
            string[] selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter,
            bool overWriteTests = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path is null");

            if (!Directory.Exists(path))
                throw new ArgumentException("path is not valid");

            if (!selectedAssembly.Any())
                throw new ArgumentException("selectedAssemly not given");


            baseUnitTestWriter
                .SetCreationPath(path)
                .SetAssemblies(selectedAssembly)
                .OverwriteTests(overWriteTests)
                .Write();
        }

        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="selectedAssembly">key : Assembly Name , Value : path of unitTest creation</param>
        /// <param name="assertType">Select Assert Type</param>
        /// <param name="unitTestFrameworkType">Select unitTest Framework</param>
        public static void CreateUnitTestFilesFromAssemblies(
            IDictionary<string, string> selectedAssembly,
            AssertType assertType = AssertType.Assert,
            UnitTestFrameworkType unitTestFrameworkType = UnitTestFrameworkType.XUnit,
            bool overWriteTests = false)
        {
            foreach (var assembly in selectedAssembly)
            {
                CreateUnitTestFilesFromAssemblies(
                    assembly.Value,
                    new[] { assembly.Key },
                    assertType,
                    unitTestFrameworkType,
                    overWriteTests);
            }
        }

        /// <summary>
        /// Create Unit Test For Enums
        /// </summary>
        /// <param name="selectedAssembly">key : Assembly Name , Value : path of unitTest creation</param>
        /// <param name="baseUnitTestWriter"></param>
        public static void CreateUnitTestFilesFromAssemblies(
            IDictionary<string, string> selectedAssembly,
            BaseUnitTestWriter baseUnitTestWriter,
            bool overWriteTests = false)
        {
            foreach (var assembly in selectedAssembly)
            {
                CreateUnitTestFilesFromAssemblies(
                    assembly.Value,
                    new[] { assembly.Key },
                    baseUnitTestWriter,
                    overWriteTests);
            }
        }
    }
}