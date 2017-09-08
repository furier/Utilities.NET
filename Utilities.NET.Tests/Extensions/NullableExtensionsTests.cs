using System;
using FluentAssertions;
using NUnit.Framework;
using Utilities.NET.Extensions;

namespace Utilities.NET.Tests.Extensions
{
    public class NullableExtensionsTests : UnitTestBase
    {
        [Test]
        [TestCase(null, ExpectedResult = true)]
        [TestCase(0, ExpectedResult = false)]
        [TestCase(42, ExpectedResult = false)]
        public bool HasNoValue(int? value) => value.HasNoValue();

        [Test]
        public void NullableDateTime_HasNoValue()
        {
            DateTime? value = null;

            value.HasNoValue().Should().BeTrue();

            value = DateTime.Now;

            value.HasNoValue().Should().BeFalse();
        }
    }
}