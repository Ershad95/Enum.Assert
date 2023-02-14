using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace TestCreator.Core;

public static class Helper
{
    private static AssertType _assertType = AssertType.Assert;
    
    internal static void SetAssertType(AssertType assertType)
    {
        _assertType = assertType;
    }
    internal static void CreateUnitTestFile(string path, IEnumerable<string> selectedAssembly)
    {
        foreach (var @enum in GetEnumsFromAssemblies(selectedAssembly))
        {
            if (File.Exists(@$"{path}/{@enum.Name}.cs")) continue;
            var unitTestFileContent = CreateContentOfFile(@enum);
            File.WriteAllText(@$"{path}/{@enum.Name}UnitTest.cs",unitTestFileContent );
        }
    }
    
    private static string CreateContentOfFile(Type @enum)
    {
        var fileContent = new StringBuilder();
        WriteNamespace(fileContent,@enum);
        WriteClassTest(fileContent,@enum);
        fileContent.Append("\n}");
        return fileContent.ToString();
    }
    
    private static void WriteNamespace(StringBuilder fileContent,Type @enum)
    {
        var nameSpace = @enum.FullName!.Replace($".{@enum.Name}", string.Empty);
        fileContent.Append($"namespace unitTest.{@enum.Name};\n");
        fileContent.Append($"using {nameSpace};\n");
    }
    private static void WriteClassTest(StringBuilder fileContent,Type @enum)
    {
        fileContent.Append($"public class {@enum.Name}UnitTest \n{{ \n");
        foreach (var item in @enum.GetEnumValues())
        {
            var titleItem = (string)Convert.ChangeType(item, typeof(string));
            var valueItem = (long)Convert.ChangeType(item, typeof(long));
            WriteMethodTest(fileContent, titleItem, valueItem,@enum);
        }
    }
    private static void WriteMethodTest(StringBuilder fileContent,string titleItem,long valueItem,MemberInfo @enum)
    {
        fileContent.Append("\n\t[Fact]\n");
        fileContent.Append($"\tpublic void {@enum.Name}_Check{titleItem}Value_ValueEqualsTo{valueItem}()\n{{");
        var type = @enum.DeclaringType is null ? @enum.Name : $"{@enum.DeclaringType.Name}.{@enum.Name}";
        
        fileContent.Append(@$"
        // Arrange
        const int {titleItem} = {valueItem};
        const {type} {@enum.Name.ToLower()} = {type}.{titleItem};
        // Act 
        const bool actual = {@enum.Name.ToLower()} == ({type}){titleItem};
        // Assert 
        {WriteAssert()};");
        fileContent.Append('\n');
        fileContent.Append("\n}");
    }
    private static string WriteAssert()
    {
        return _assertType switch
        {
            AssertType.Assert => "Assert.True(actual)",
            AssertType.Shouldly => "actual.ShouldBeTrue()",
            AssertType.FluentAssertions => "actual.Should().Be(true)",
            _ => throw new InvalidEnumArgumentException()
        };
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