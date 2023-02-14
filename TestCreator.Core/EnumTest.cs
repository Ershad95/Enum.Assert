namespace TestCreator.Core
{
    public static class EnumTest
    {
        public static void WriteUnitTest(string path, string[] selectedAssembly)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path is null");

            if (!Directory.Exists(path))
                throw new ArgumentException("path is not valid");

            if (!selectedAssembly.Any())
                throw new ArgumentException("selectedAssemly not given");

            Helper.CreateUnitTestFile(path,selectedAssembly);
        }
    }
}