using System.Reflection;
using System.Text;

namespace TestCreator.Core;

public static class Helper
{
    internal static void CreateUnitTestFile(string path, IEnumerable<string> selectedAssembly)
    {
        foreach (var @enum in GetEnumsFromAssemblies(selectedAssembly))
        {
            if (File.Exists(@$"{path}/{@enum.Name}.cs")) continue;
            File.WriteAllText(@$"{path}/{@enum.Name}UnitTest.cs", CreateFileContent(@enum));
        }
    }

    private static string CreateFileContent(Type @enum)
    {
        var fileContent = new StringBuilder();
        var minValueItem = long.MaxValue;
        var maxValueItem = long.MinValue;

        var nameSpace = @enum.FullName!.Replace($".{@enum.Name}", string.Empty);
        fileContent.Append($"namespace unitTest.{@enum.Name};\nusing {nameSpace};\n");
        fileContent.Append($"public class {@enum.Name}UnitTest \n{{ \n");


        foreach (var item in @enum.GetEnumValues())
        {
            var valueItem = (long)Convert.ChangeType(item, typeof(long));
            var titleItem = (string)Convert.ChangeType(item, typeof(string));

            if (minValueItem > maxValueItem) minValueItem = valueItem;
            if (maxValueItem < valueItem) maxValueItem = valueItem;

            fileContent.Append("\n\n\t[Fact]\n\n");

            fileContent.Append($"\tpublic void {@enum.Name}_Check{titleItem}Value_ValueEqualsTo{valueItem}()\n{{");
            var type = @enum.DeclaringType is null ? @enum.Name : $"{@enum.DeclaringType.Name}.{@enum.Name}";
            fileContent.Append(@$"
        // Arrange
        const int {titleItem} = {valueItem};
        const {type} {@enum.Name.ToLower()} = {type}.{titleItem};
        // Act 
        const bool actual = {@enum.Name.ToLower()} == ({type}){titleItem};
        // Assert 
        Assert.True(actual);");
            fileContent.Append("\n");
            fileContent.Append("\n}}");
        }

        fileContent.Append($"\n}}");
        return fileContent.ToString();
    }

    private static IEnumerable<TypeInfo> GetEnumsFromAssemblies(IEnumerable<string> selectedAssembly)
    {
        var allAssembly = AppDomain.CurrentDomain.GetAssemblies();
        if (allAssembly is null)
            throw new ArgumentException("EntryAssembly is null");
        var assemblyTarget = new List<Assembly>();
        foreach (var name in selectedAssembly)
        {
            assemblyTarget.AddRange(allAssembly.Where(x => x.FullName!.Contains(name)));
        }

        var typeInfos = new List<TypeInfo>();
        foreach (var assembly in assemblyTarget)
        {
            typeInfos.AddRange(assembly.DefinedTypes.Where(x => x.IsEnum));
        }

        return typeInfos;
    }
}