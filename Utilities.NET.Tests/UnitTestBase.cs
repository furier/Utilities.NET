using NUnit.Framework;

namespace Utilities.NET.Tests
{
    [TestFixture]
    public abstract class UnitTestBase
    {
        [SetUp]
        public virtual void SetUp() { }

        [TearDown]
        public virtual void TearDown() { }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp() { }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown() { }
    }
}