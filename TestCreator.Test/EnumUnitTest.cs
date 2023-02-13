using lib;
using System;
using Xunit;

namespace TestCreator.Test
{
    public class EnumUnitTest
    {
        [Fact]
        public void WriteUnitTest_PathIsNull_ThrowArgumentNullException()
        {
            // Arrange
            string path = null;
            // Act
            Action action = () =>
            {
                EnumTest.WriteUnitTest(path, new string[] { "" });
            };
            // Assert 
            Assert.Throws<ArgumentNullException>(action);
        }
        [Fact]
        public void WriteUnitTest_PathIsEmptyString_ThrowArgumentNullException()
        {
            // Arrange
            string path = string.Empty;
            // Act
            Action action = () =>
            {
                EnumTest.WriteUnitTest(path,new string[] {""});
            };
            // Assert 
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void WriteUnitTest_PathIsNotExist_ThrowArgumentNullException()
        {
            // Arrange
            string path = "ljdkfjdkfdf";
            // Act
            Action action = () =>
            {
                EnumTest.WriteUnitTest(path, new string[] { "" });
            };
            // Assert 
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void WriteUnitTest_AsseblyIsEmpty_ThrowArgumentException()
        {
            // Arrange
            string path = Environment.CurrentDirectory;
            // Act
            Action action = () =>
            {
                EnumTest.WriteUnitTest(path, new string[] {});
            };
            // Assert 
            Assert.Throws<ArgumentException>(action);
        }
    }
}