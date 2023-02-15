using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestCreator.Core;

public class WriteUnitTestHelper
{
    private AssertType _assertType = AssertType.Assert;
    private UnitTestFrameworkType _testFrameworkType = UnitTestFrameworkType.XUnit;

    internal void SetAssertType(AssertType assertType)
    {
        _assertType = assertType;
    }

    internal void SetUnitTestFrameworkType(UnitTestFrameworkType unitTestFrameworkType)
    {
        _testFrameworkType = unitTestFrameworkType;
    }

    internal void CreateUnitTestFile(string path, IEnumerable<string> selectedAssembly)
    {
        foreach (var @enum in GetEnumsFromAssemblies(selectedAssembly))
        {
            if (File.Exists(@$"{path}/{@enum.Name}.cs")) continue;
            var unitTestFileContent = CreateContentOfFile(@enum);
            File.WriteAllText(@$"{path}/{@enum.Name}UnitTest.cs", unitTestFileContent);
        }
    }

    private string CreateContentOfFile(Type @enum)
    {
        var fileContent = new StringBuilder();
        WriteNamespace(fileContent, @enum);
        WriteClassTest(fileContent, @enum);
        fileContent.Append("\n}");
        return fileContent.ToString();
    }

    protected virtual void WriteClassTest(StringBuilder fileContent, Type @enum)
    {
        WriteClassAttribute(fileContent);
        fileContent.Append($"public class {@enum.Name}UnitTest \n{{ \n");
        foreach (var item in @enum.GetEnumValues())
        {
            var titleItem = (string)Convert.ChangeType(item, typeof(string));
            var valueItem = (long)Convert.ChangeType(item, typeof(long));
            WriteMethodTest(fileContent, titleItem, valueItem, @enum);
        }
    }
    protected virtual void WriteMethodTest(StringBuilder fileContent, string enumItem, long valueItem, MemberInfo @enum)
    {
        WriteMethodAttribute(fileContent);
        fileContent.Append($"\tpublic void {@enum.Name}_Check{enumItem}Value_ValueEqualsTo{valueItem}()\n{{");

        var type = @enum.DeclaringType is null ? @enum.Name : $"{@enum.DeclaringType.Name}.{@enum.Name}";
        fileContent.Append(@$"
        // Arrange
        const int {enumItem} = {valueItem};
        const {type} {@enum.Name.ToLower()} = {type}.{enumItem};
        // Act 
        const bool actual = {@enum.Name.ToLower()} == ({type}){enumItem};
        // Assert 
        {WriteAssertOperation()};");
        fileContent.Append('\n');
        fileContent.Append("\n}");
    }

    private string WriteAssertOperation()
    {
        return _assertType switch
        {
            AssertType.Assert => "Assert.True(actual)",
            AssertType.Shouldly => "actual.ShouldBeTrue()",
            AssertType.FluentAssertions => "actual.Should().Be(true)",
            _ => throw new InvalidEnumArgumentException()
        };
    }

    private void WriteMethodAttribute(StringBuilder fileContent)
    {
        switch (_testFrameworkType)
        {
            case UnitTestFrameworkType.XUnit:
                break;
            case UnitTestFrameworkType.NUnit:
                fileContent.Append("\n\t[TestFixture]\n");
                break;
            case UnitTestFrameworkType.MsUnit:
                fileContent.Append("\n\t[TestClass]\n");
                break;
            default:
                throw new ConstraintException();
        }
    }

    private void WriteClassAttribute(StringBuilder fileContent)
    {
        switch (_testFrameworkType)
        {
            case UnitTestFrameworkType.XUnit:
                fileContent.Append("\n\t[Fact]\n");
                break;
            case UnitTestFrameworkType.NUnit:
                fileContent.Append("\n\t[Test]\n");
                break;
            case UnitTestFrameworkType.MsUnit:
                fileContent.Append("\n\t[TestMethod]\n");
                break;
            default:
                throw new ConstraintException();
        }
    }

    private void WriteNamespace(StringBuilder fileContent, Type @enum)
    {
        fileContent.Append($"namespace unitTest.{@enum.Name};\n");
        WriteEnumNameSpace(fileContent, @enum);
        WriteAssertNameSpace(fileContent);
    }

    private void WriteEnumNameSpace(StringBuilder fileContent, Type @enum)
    {
        var enumNameSpace = @enum.FullName!.Replace($".{@enum.Name}", string.Empty);
        fileContent.Append($"using {enumNameSpace};\n");
    }

    private void WriteAssertNameSpace(StringBuilder fileContent)
    {
        switch (_assertType)
        {
            case AssertType.Assert:
                break;
            case AssertType.Shouldly:
                fileContent.Append("using using Shouldly;\n");
                break;
            case AssertType.FluentAssertions:
                fileContent.Append("using FluentAssertions;\n");
                break;
            default:
                throw new ConstraintException();
        }
    }

    private IEnumerable<TypeInfo> GetEnumsFromAssemblies(IEnumerable<string> selectedAssembly)
    {
        var allDependency = AppDomain.CurrentDomain.GetAssemblies();
        if (allDependency is null)
            throw new ArgumentException("EntryAssembly is null");

        var assemblies = new List<Assembly>();
        foreach (var name in selectedAssembly)
        {
            assemblies.AddRange(allDependency.Where(x => x.FullName!.Contains(name)));
        }

        var allEnums = new List<TypeInfo>();
        foreach (var assembly in assemblies)
        {
            allEnums.AddRange(assembly.DefinedTypes.Where(x => x.IsEnum));
        }

        return allEnums;
    }
}