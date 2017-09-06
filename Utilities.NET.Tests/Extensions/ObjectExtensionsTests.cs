using System;
using System.Runtime.Serialization;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using Utilities.NET.Extensions;

namespace Utilities.NET.Tests.Extensions
{
    public class ObjectExtensionsTests
    {
        private enum FooBar
        {
            [EnumMember(Value = "FOO BAR")]
            Foo = -1,
            Bar = 1
        }

        [Test]
        [TestCase(FooBar.Foo, ExpectedResult = FooBar.Foo)]
        [TestCase(FooBar.Bar, ExpectedResult = FooBar.Bar)]
        [TestCase("Foo", ExpectedResult = FooBar.Foo)]
        [TestCase("Bar", ExpectedResult = FooBar.Bar)]
        [TestCase(-1, ExpectedResult = FooBar.Foo)]
        [TestCase(1, ExpectedResult = FooBar.Bar)]
        [TestCase(null, ExpectedResult = default(FooBar))]
        public object ChangeTypeEnum(object @object)
        {
            return @object.ChangeType<FooBar>();
        }

        [Test]
        [TestCase(FooBar.Foo, ExpectedResult = -1)]
        [TestCase(FooBar.Bar, ExpectedResult = 1)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase("1", ExpectedResult = 1)]
        public object ChangeTypeInt(object @object)
        {
            return @object.ChangeType<int>();
        }

        [Test]
        [TestCase(FooBar.Foo, ExpectedResult = "Foo")]
        [TestCase(1, ExpectedResult = "1")]
        [TestCase("1", ExpectedResult = "1")]
        public object ChangeTypeString(object @object)
        {
            return @object.ChangeType<string>();
        }

        [Test]
        public void ChangeTypeDateTime()
        {
            var now = DateTime.Now;

            object @object = now.ToString("O");

            var dateTime = @object.ChangeType<DateTime>();

            Assert.AreEqual(dateTime, now);
        }

        [Test]
        [TestCase(null, ExpectedResult = 0.0d)]
        [TestCase("", ExpectedResult = 0.0d)]
        [TestCase("0", ExpectedResult = 0.0d)]
        [TestCase("1", ExpectedResult = 1.0d)]
        [TestCase("1.0", ExpectedResult = 1.0d)]
        [TestCase("2.2", ExpectedResult = 2.2d)]
        [TestCase("2,2", ExpectedResult = 2.2d)]
        public object ChangeTypeDouble(object @object)
        {
            return @object.ChangeType<double>();
        }

        [Test]
        [TestCase(null, ExpectedResult = 0.0f)]
        [TestCase("", ExpectedResult = 0.0f)]
        [TestCase("0", ExpectedResult = 0.0f)]
        [TestCase("1", ExpectedResult = 1.0f)]
        [TestCase("1.0", ExpectedResult = 1.0f)]
        [TestCase("2.2", ExpectedResult = 2.2f)]
        [TestCase("2,2", ExpectedResult = 2.2f)]
        public object ChangeTypeFloat(object @object)
        {
            return @object.ChangeType<float>();
        }

        public class Person
        {
            public Person(string name, int age, DateTime dateOfBirth, double weight, float fatPercentage)
            {
                Name = name;
                Age = age;
                DateOfBirth = dateOfBirth;
                Weight = weight;
                _fatPercentage = fatPercentage;
            }

            public Person() { }

            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime DateOfBirth { get; set; }
            private double Weight { get; set; }
            private float _fatPercentage;
        }

        [Test]
        [AutoData]
        public void ToDictionary(Person person)
        {
            var dictionary = person.ToDictionary(
                x => x.Name, 
                x => x.DateOfBirth
            );

            dictionary.Should().HaveCount(2);
            dictionary["Name"].Should().BeOfType<string>().And.Be(person.Name);
            dictionary["DateOfBirth"].Should().BeOfType<DateTime>().And.Be(person.DateOfBirth);
        }

        [Test]
        [AutoData]
        public void ToDictionaryAll(Person person)
        {
            var dictionary = person.ToDictionaryAll();

            dictionary.Should().HaveCount(3);
            dictionary["Name"].Should().BeOfType<string>().And.Be(person.Name);
            dictionary["Age"].Should().BeOfType<int>().And.Be(person.Age);
            dictionary["DateOfBirth"].Should().BeOfType<DateTime>().And.Be(person.DateOfBirth);
        }

        [Test]
        public void ToDictionary_OnNullObject_ShouldReturnEmptyDictionary()
        {
            Person person = null;

            var dictionary = person.ToDictionary(
                x => x.Name,
                x => x.DateOfBirth
            );

            dictionary.Should().HaveCount(0);
        }

        [Test]
        [AutoData]
        public void GetPropValue(string name, int age, DateTime dateOfBirth, double weight, float fatPercentage)
        {
            var person = new Person(name, age, dateOfBirth, weight, fatPercentage);

            var weightPropValue = person.GetPropertyValue("Weight");

            weightPropValue.Should().Be(weight);

            var fatPercentagePropValue = person.GetPropertyValue("_fatPercentage");

            fatPercentagePropValue.Should().Be(fatPercentage);

            var agePropValue = person.GetPropertyValue("Age");

            agePropValue.Should().Be(age);

            person.GetPropertyValue("ASDASD").Should().BeNull("The property does not exist.");

            var dateOfBirthPropValue = person.GetPropertyValue<DateTime>("DateOfBirth");
            
            dateOfBirthPropValue.Should().Be(dateOfBirth);

            Assert.Throws<InvalidCastException>(() => person.GetPropertyValue<int>("DateOfBirth"), "The property DateOfBirth is not of type DateTime.");

            person.GetPropertyValue<int>("DateOfBirth", true).Should().Be(default(int), "Silent is set to true.");
        }
    }
}