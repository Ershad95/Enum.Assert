using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;

namespace TestCreator.Core
{
    public abstract class BaseUnitTestWriter
    {
        private AssertType _assertType = AssertType.Assert;
        private UnitTestFrameworkType _testFrameworkType = UnitTestFrameworkType.XUnit;
        private IEnumerable<string> _selectedAssembly;
        private string _path = string.Empty;
        private bool _overWriteTests;

        internal BaseUnitTestWriter SetTestAssertType(AssertType assertType)
        {
            _assertType = assertType;
            return this;
        }
        internal BaseUnitTestWriter SetTestFrameworkType(UnitTestFrameworkType unitTestFrameworkType)
        {
            _testFrameworkType = unitTestFrameworkType;
            return this;
        }
        internal BaseUnitTestWriter SetCreationPath(string path)
        {
            _path = path;
            return this;
        }
        internal BaseUnitTestWriter SetAssemblies(IEnumerable<string> selectedAssembly)
        {
            _selectedAssembly = selectedAssembly;
            return this;
        }
        internal BaseUnitTestWriter OverwriteTests(bool overWriteTests)
        {
            _overWriteTests = overWriteTests;
            return this;
        }
        internal (bool status, Exception exception) Write()
        {
            try
            {
                foreach (var @enum in GetEnumsFromAssemblies(_selectedAssembly))
                {
                    if (!_overWriteTests && File.Exists(@$"{_path}/{@enum.Name}.cs")) continue;
                    var unitTestFileContent = CreateContentOfFile(@enum);
                    File.WriteAllText(@$"{_path}/{@enum.Name}UnitTest.cs", unitTestFileContent);
                }
                return (true, null);
            }
            catch (Exception exception)
            {
                return (false, exception);
            }
        }

        /// <summary>
        /// you can customize generation class
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="enum"></param>
        protected virtual void WriteClassTest(StringBuilder fileContent, Type @enum)
        {
            WriteClassAttribute(fileContent);
            fileContent.Append($"\n    public class {@enum.Name}UnitTest \n    {{\n");
            foreach (var item in @enum.GetEnumValues())
            {
                var titleItem = (string)Convert.ChangeType(item, typeof(string));
                var valueItem = (long)Convert.ChangeType(item, typeof(long));
                WriteMethodTest(fileContent, titleItem, valueItem, @enum);
            }

            fileContent.Append(" \n    }");
        }
        /// <summary>
        /// you can customize all of the test methods
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="enumItem"></param>
        /// <param name="valueItem"></param>
        /// <param name="enum"></param>
        protected virtual void WriteMethodTest(StringBuilder fileContent,
            string enumItem,
            long valueItem,
            MemberInfo @enum)
        {
            WriteMethodAttribute(fileContent);
            var convertNumberToText = ConvertNumberToText(valueItem);
            var type = @enum.DeclaringType is null ? @enum.Name : $"{@enum.DeclaringType.Name}.{@enum.Name}";
            fileContent.Append($"        public void " +
               $"{@enum.Name}_Check{enumItem}Value_ValueEqualsTo{convertNumberToText}()" +
               $"\n        {{\n");

            var parts = type.Split(".");
            var enumVariableName = parts[parts.Length - 1];
            var firstCharacter = enumVariableName.First().ToString().ToLower();
            enumVariableName = $"{firstCharacter}{enumVariableName[1..enumVariableName.Length]}";
            fileContent.Append(
                $"            // Arrange\n " +
                $"           const int {enumItem.ToLower()} = {valueItem};\n" +
                $"            const {type} {enumVariableName} = {type}.{enumItem};\n" +
                $"            // Act\n " +
                $"           const bool actual = {enumVariableName} == ({type}){enumItem.ToLower()};\n" +
                $"            // Assert \n" +
                $"            {WriteAssertOperation()};");

            fileContent.Append("\n        }");
            fileContent.Append('\n');
        }
        protected virtual void WriteNamespace(StringBuilder fileContent, Type @enum)
        {
            WriteTestFrameWorkNameSpace(fileContent);
            WriteEnumNameSpace(fileContent, @enum);
            WriteAssertNameSpace(fileContent);
            fileContent.Append($"namespace unitTest_{@enum.Name}\n{{");
        }

        private static void WriteEnumNameSpace(StringBuilder fileContent, Type @enum)
        {
            fileContent.Append($"using {@enum.Namespace};\n");
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
                    fileContent.Append("\n        [Fact]\n");
                    break;
                case UnitTestFrameworkType.NUnit:
                    fileContent.Append("\n        [TestFixture]\n");
                    break;
                case UnitTestFrameworkType.MsUnit:
                    fileContent.Append("\n        [TestClass]\n");
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
        private void WriteTestFrameWorkNameSpace(StringBuilder fileContent)
        {
            switch (_testFrameworkType)
            {
                case UnitTestFrameworkType.XUnit:
                    fileContent.Append("using Xunit;\n");
                    break;
                case UnitTestFrameworkType.NUnit:
                    break;
                case UnitTestFrameworkType.MsUnit:
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
                allEnums.AddRange(assembly.DefinedTypes.Where(x => x.IsEnum && x.IsVisible));
            }

            return allEnums;
        }
        private static string ConvertNumberToText(long valueItem)
        {
            var positiveValue = Math.Abs(valueItem);
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
                _ => positiveValue.ToString()
            };

            if (valueItem < 0)
                result = $"Negative{result}";
            return result;
        }
        private string CreateContentOfFile(Type @enum)
        {
            var fileContent = new StringBuilder();
            WriteNamespace(fileContent, @enum);
            WriteClassTest(fileContent, @enum);
            fileContent.Append("\n}");
            return fileContent.ToString();
        }
    }
}