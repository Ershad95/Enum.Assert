using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lib
{
    public static class EnumTest
    {
        public static void WriteUnitTest(string path,string[] selectedAssemly)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path is null");

            if (!Directory.Exists(path))
                throw new ArgumentNullException("path is not valid");

            if(!selectedAssemly.Any())
                throw new ArgumentException("selectedAssemly not given");



            foreach (var @enum in GetEnumsFromEntryAssembly(selectedAssemly))
            {               
                if (File.Exists(@$"{path}/{@enum.Name}.cs")) continue;
                var content = CreateFileContent(@enum);
                File.WriteAllText(@$"{path}/{@enum.Name}.cs", content);
            }

            string CreateFileContent(TypeInfo @enum)
            {
                var fileContent = new StringBuilder();
                long minValueItem = long.MaxValue;
                long maxValueItem = long.MinValue;

                fileContent.Append($"namespace unitTest.{@enum.Name};\nusing {AppDomain.CurrentDomain.FriendlyName};\n");
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
        actual.ShouldBeTrue();");
                    fileContent.Append("\n");
                    fileContent.Append($"\n}}");
                }
                fileContent.Append($"\n}}");
                return fileContent.ToString();
            }
        }

        private static IEnumerable<TypeInfo> GetEnumsFromEntryAssembly(string[] selectedAssemly)
        {
            var allAssebly = AppDomain.CurrentDomain.GetAssemblies();
            if (allAssebly is null)
                throw new NullReferenceException("EntryAssembly is null");

            List<TypeInfo> typeInfos = new List<TypeInfo>();
            List<Assembly> assemblyTarget = new List<Assembly>();
            foreach (var name in selectedAssemly)
            {
                assemblyTarget.AddRange(allAssebly.Where(x => x.FullName!.Contains(name)));
            }

            foreach (var assembly in assemblyTarget)
            {
                typeInfos.AddRange(assembly.DefinedTypes.Where(x => x.IsEnum));
            }
            return typeInfos;
        }
    }
}
