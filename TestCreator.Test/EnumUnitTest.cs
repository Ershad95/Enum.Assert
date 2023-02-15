using System;
using TestCreator.Core;
using Xunit;

namespace TestCreator.Test
{
    public class EnumUnitTest
    {
        [Fact]
        public void WriteUnitTest_PathIsNull_ThrowArgumentNullException()
        {
            // Arrange
            string path = null!;
            // Act
            void Action()
            {
                EnumTest.CreateUnitTestFilesFromAssemblies(path, new string[] { "" });
            }

            // Assert 
            Assert.Throws<ArgumentException>((Action)Action);
        }
        [Fact]
        public void WriteUnitTest_PathIsEmptyString_ThrowArgumentNullException()
        {
            // Arrange
            var path = string.Empty;
            // Act
            void Action() => EnumTest.CreateUnitTestFilesFromAssemblies(path, new[] { "" });

            // Assert 
            Assert.Throws<ArgumentException>((Action)Action);
        }

        [Fact]
        public void WriteUnitTest_PathIsNotExist_ThrowArgumentNullException()
        {
            // Arrange
            const string path = "ljdkfjdkfdf";
            // Act
            void Action()
            {
                EnumTest.CreateUnitTestFilesFromAssemblies(path, new[] { "" });
            }

            // Assert 
            Assert.Throws<ArgumentException>((Action)Action);
        }

        [Fact]
        public void WriteUnitTest_AssemblyIsEmpty_ThrowArgumentException()
        {
            // Arrange
            var path = Environment.CurrentDirectory;
            // Act
            void Action()
            {
                EnumTest.CreateUnitTestFilesFromAssemblies(path, Array.Empty<string>());
            }

            // Assert 
            Assert.Throws<ArgumentException>((Action)Action);
        }
    }
}