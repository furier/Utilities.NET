using System;
using NUnit.Framework;
using Utilities.NET.Extensions;

namespace Utilities.NET.Tests.Extensions
{
    public class IntExtensionsTests : UnitTestBase
    {
        public enum MyEnum
        {
            Foo = 0,
            Bar = 1
        }

        [Test]
        [TestCase(0, ExpectedResult = MyEnum.Foo)]
        [TestCase(1, ExpectedResult = MyEnum.Bar)]
        [TestCase(3, default(MyEnum), true, ExpectedResult = MyEnum.Foo)]
        [TestCase(4, MyEnum.Bar, true, ExpectedResult = MyEnum.Bar)]
        public MyEnum ToEnum(int num, MyEnum @default = default(MyEnum), bool silent = false)
        {
            return num.ToEnum<MyEnum>(@default, silent);
        }

        [Test]
        [TestCase(3)]
        public void ToEnumShouldThrowException(int num)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => num.ToEnum<MyEnum>(@default: default(MyEnum), silent: false));
        }
    }
}