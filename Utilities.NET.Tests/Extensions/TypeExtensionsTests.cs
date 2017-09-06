using System;
using NUnit.Framework;
using Utilities.NET.Extensions;

namespace Utilities.NET.Tests.Extensions
{
    public class TypeExtensionsTests : UnitTestBase
    {
        private enum MyEnum
        {
            Foo = 0,
            Bar = 1
        }

        [Test]
        [TestCase(typeof(string), ExpectedResult = null)]
        [TestCase(typeof(int), ExpectedResult = 0)]
        [TestCase(typeof(Enum), ExpectedResult = null)]
        [TestCase(typeof(MyEnum), ExpectedResult = MyEnum.Foo)]
        public object GetDefaultValue(Type type)
        {
            return type.GetDefaultValue();
        }

        [Test]
        public void GetDefaultValue()
        {
            var type = typeof(DateTime);

            var @default = type.GetDefaultValue();

            Assert.AreEqual(@default, default(DateTime));
        }
    }
}