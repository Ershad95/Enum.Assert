using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;

namespace TestCreator.Core
{
    public class BaseUnitTestWriter
    {
        private AssertType _assertType = AssertType.Assert;
        private UnitTestFrameworkType _testFrameworkType = UnitTestFrameworkType.XUnit;

        public void SetAssertType(AssertType assertType)
        {
            _assertType = assertType;
        }

        public void SetUnitTestFrameworkType(UnitTestFrameworkType unitTestFrameworkType)
        {
            _testFrameworkType = unitTestFrameworkType;
        }
        
        public void CreateUnitTestFile(string path, IEnumerable<string> selectedAssembly)
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

            fileContent.Append("\n}");
        }
        
        protected virtual void WriteMethodTest(StringBuilder fileContent, string enumItem, long valueItem, MemberInfo @enum)
        {
            WriteMethodAttribute(fileContent);
            var convertNumberToText = ConvertNumberToText(valueItem);
            fileContent.Append($"public void {@enum.Name}_Check{enumItem}Value_ValueEqualsTo{convertNumberToText}()\n{{");

            var type = @enum.DeclaringType is null ? @enum.Name : $"{@enum.DeclaringType.Name}.{@enum.Name}";
            fileContent.Append(@$"
        // Arrange
        const int {enumItem.ToLower()} = {valueItem};
        const {type} {@enum.Name.ToLower()} = {type}.{enumItem};
        // Act 
        const bool actual = {@enum.Name.ToLower()} == ({type}){enumItem.ToLower()};
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
            WriteEnumNameSpace(fileContent, @enum);
            WriteAssertNameSpace(fileContent);
            fileContent.Append($"namespace unitTest_{@enum.Name}\n{{");
        }

        private static void WriteEnumNameSpace(StringBuilder fileContent, Type @enum)
        {
            var enumNameSpace = @enum.FullName!.Replace($".{@enum.Name}", string.Empty);
            fileContent.Append($"using {enumNameSpace};\n");
        }

        private void WriteAssertNameSpace(StringBuilder fileContent)
        {
            switch (_assertType)
            {
                case AssertType.Assert:
                    fileContent.Append("using Xunit;\n");
                    break;
                case AssertType.Shouldly:
                    fileContent.Append("using Shouldly;\n");
                    break;
                case AssertType.FluentAssertions:
                    fileContent.Append("using FluentAssertions;\n");
                    break;
                default:
                    throw new ConstraintException();
            }
        }

        private static IEnumerable<TypeInfo> GetEnumsFromAssemblies(IEnumerable<string> selectedAssembly)
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
        private static string ConvertNumberToText(long valueItem)
        {
            var positive = Math.Abs(valueItem);
            var result = Math.Abs(valueItem) switch
            {
                0 => "Zero",
                1 => "One",
                2 => "Two",
                3 => "Three",
                4 => "Four",
                5 => "Five",
                6 => "six",
                7 => "Seven",
                8 => "Eight",
                9 => "Nine",
                _ => positive.ToString()
            };

            if (valueItem < 0)
                result = $"Negative{result}";
            return result;
        }
    }
}