using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using Utilities.NET.Extensions;

namespace Utilities.NET.Tests.Extensions
{
    public class EnumerableExtensionsTests : UnitTestBase
    {
        private void AreEqual_ShouldBeEqual(IEnumerable<string> enumerableA, IEnumerable<string> enumerableB, bool expected)
        {
            enumerableA.AreEqual(enumerableB).Should().Be(expected);
        }

        [Test]
        public void AreEqual()
        {
            AreEqual_ShouldBeEqual(new List<string>(), new List<string>(), true);
            AreEqual_ShouldBeEqual(new List<string> { "A" }, new List<string> { "A" }, true);
            AreEqual_ShouldBeEqual(new List<string> { "A" }, new List<string> { "B" }, false);
            AreEqual_ShouldBeEqual(new List<string> { "A", "B" }, new List<string> { "A", "B" }, true);
            AreEqual_ShouldBeEqual(new List<string> { "B", "A" }, new List<string> { "A", "B" }, false);

            AreEqual_ShouldBeEqual(new string[0], new string[0], true);
            AreEqual_ShouldBeEqual(new[] { "A" }, new[] { "A" }, true);
            AreEqual_ShouldBeEqual(new[] { "A" }, new[] { "B" }, false);
            AreEqual_ShouldBeEqual(new[] { "A", "B" }, new[] { "A", "B" }, true);
            AreEqual_ShouldBeEqual(new[] { "B", "A" }, new[] { "A", "B" }, false);

            AreEqual_ShouldBeEqual(new Collection<string>(), new Collection<string>(), true);
            AreEqual_ShouldBeEqual(new Collection<string> { "A" }, new Collection<string> { "A" }, true);
            AreEqual_ShouldBeEqual(new Collection<string> { "A" }, new Collection<string> { "B" }, false);
            AreEqual_ShouldBeEqual(new Collection<string> { "A", "B" }, new Collection<string> { "A", "B" }, true);
            AreEqual_ShouldBeEqual(new Collection<string> { "B", "A" }, new Collection<string> { "A", "B" }, false);
        }

        [Test]
        [TestCase(1000, 100, ExpectedResult = 10)]
        [TestCase(150, 100, ExpectedResult = 2)]
        [TestCase(100, 100, ExpectedResult = 1)]
        [TestCase(10, 100, ExpectedResult = 1)]
        public int Split(int totalCount, int splitSize)
        {
            var list = new Fixture().CreateMany<string>(totalCount).ToList();

            var split = list.Split(splitSize)?.ToList();

            split.Should().NotBeNull();

            return split.Count;
        }

        [Test]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = false)]
        [TestCase(99, ExpectedResult = false)]
        public bool EmptyArray(int count)
        {
            var array = new string[count];

            return array.Empty();
        }

        [Test]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = false)]
        [TestCase(99, ExpectedResult = false)]
        public bool EmptyList(int count)
        {
            var list = Enumerable.Range(0, count).ToList();

            return list.Empty();
        }

        [Test]
        public void None()
        {
            var list = new List<string> { "ASD", "DSA" };

            list.None(x => x == "123").Should().BeTrue();
            list.None(x => x == "ASD").Should().BeFalse();
        }
    }
}