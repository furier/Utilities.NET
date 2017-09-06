using System;
using NUnit.Framework;
using Utilities.NET.Extensions;

namespace Utilities.NET.Tests.Extensions
{
    public class StringExtensionsTests : UnitTestBase
    {
        [Test]
        [TestCase("trim this string", "string", ExpectedResult = "trim this ")]
        [TestCase("trim this string", " this string", ExpectedResult = "trim")]
        [TestCase("trim this string ", "string", ExpectedResult = "trim this string ")]
        [TestCase("trim this string  ", "string ", ExpectedResult = "trim this string  ")]
        public string TrimEnd(string @string, string toTrim)
        {
            return @string.TrimEnd(toTrim);
        }

        [Test]
        [TestCase("not an empty string", ExpectedResult = false)]
        [TestCase("not an empty string  ", ExpectedResult = false)]
        [TestCase("  not an empty string", ExpectedResult = false)]
        [TestCase("  not an empty string  ", ExpectedResult = false)]
        [TestCase(null, ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        public bool IsNullOrEmpty(string @string)
        {
            return @string.IsNullOrEmpty();
        }

        [Test]
        [TestCase("not an empty string", ExpectedResult = false)]
        [TestCase("not an empty string  ", ExpectedResult = false)]
        [TestCase("  not an empty string", ExpectedResult = false)]
        [TestCase("  not an empty string  ", ExpectedResult = false)]
        [TestCase(null, ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        [TestCase(" ", ExpectedResult = true)]
        [TestCase("      ", ExpectedResult = true)]
        public bool IsNullOrWhiteSpace(string @string)
        {
            return @string.IsNullOrWhiteSpace();
        }

        [Test]
        [TestCase("not an empty string", ExpectedResult = true)]
        [TestCase("not an empty string  ", ExpectedResult = true)]
        [TestCase("  not an empty string", ExpectedResult = true)]
        [TestCase("  not an empty string  ", ExpectedResult = true)]
        [TestCase(null, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        public bool IsNotNullOrEmpty(string @string)
        {
            return @string.IsNotNullOrEmpty();
        }

        [Test]
        [TestCase("not an empty string", ExpectedResult = true)]
        [TestCase("not an empty string  ", ExpectedResult = true)]
        [TestCase("  not an empty string", ExpectedResult = true)]
        [TestCase("  not an empty string  ", ExpectedResult = true)]
        [TestCase(null, ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [TestCase(" ", ExpectedResult = false)]
        [TestCase("      ", ExpectedResult = false)]
        public bool IsNotNullOrWhiteSpace(string @string)
        {
            return @string.IsNotNullOrWhiteSpace();
        }

        [Test]
        [TestCase("FIrst Letter TO UPPER case!!!!#¤%¤#", ExpectedResult = "First letter to upper case!!!!#¤%¤#")]
        [TestCase("FIRST", ExpectedResult = "First")]
        [TestCase("FIRST LETTER", ExpectedResult = "First letter")]
        [TestCase("fIRST LETTER", ExpectedResult = "First letter")]
        [TestCase("first letter", ExpectedResult = "First letter")]
        [TestCase("   first letter", ExpectedResult = "   first letter")] //does not trim and convert the first none white space char to upper case.
        [TestCase("!#¤%&/first letter", ExpectedResult = "!#¤%&/first letter")]
        public string FirstLetterToUpper(string @string)
        {
            return @string.FirstLetterToUpper();
        }

        [Test]
        [TestCase("TO TITLE CASE", ExpectedResult = "To Title Case")]
        [TestCase("TO #TITLE CASE", ExpectedResult = "To #Title Case")]
        [TestCase("TO!!TITLE CASE", ExpectedResult = "To!!Title Case")]
        [TestCase("TO!!TITLE CASE   ", ExpectedResult = "To!!Title Case   ")]
        [TestCase("   TO!!TITLE CASE", ExpectedResult = "   To!!Title Case")]
        [TestCase("   TO!!TITLE CASE   ", ExpectedResult = "   To!!Title Case   ")]
        public string ToTitleCase(string @string)
        {
            return @string.ToTitleCase();
        }

        public enum MyEnum
        {
            Foo = 0,
            Bar = 1
        }

        [Test]
        [TestCase("Foo", ExpectedResult = MyEnum.Foo)]
        [TestCase("0", ExpectedResult = MyEnum.Foo)]
        [TestCase("Bar", ExpectedResult = MyEnum.Bar)]
        [TestCase("1", ExpectedResult = MyEnum.Bar)]
        [TestCase("Lol", default(MyEnum), true, ExpectedResult = MyEnum.Foo)]
        [TestCase("Lol", MyEnum.Bar, true, ExpectedResult = MyEnum.Bar)]
        public MyEnum ToEnum(string str, MyEnum @default = default(MyEnum), bool silent = false)
        {
            return str.ToEnum(@default, silent);
        }

        [Test]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("Lol")]
        [TestCase("3")]
        public void ToEnumShouldThrowException(string str)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => str.ToEnum(default(MyEnum), false));
        }
    }
}