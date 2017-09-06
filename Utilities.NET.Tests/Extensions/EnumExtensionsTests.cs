using System;
using System.Runtime.Serialization;
using FluentAssertions;
using NUnit.Framework;
using Utilities.NET.Extensions;

namespace Utilities.NET.Tests.Extensions
{
    public class EnumExtensionsTests : UnitTestBase
    {
        public enum FooEnum
        {
            Foo = 0,
            Bar = 1,
            Lol = 3,
            Not = 5,
            Ping = 10,
            [EnumMember(Value = "Panser")]
            Super,
            [EnumMember(Value = "Bio")]
            Bio
        }

        public enum BarEnum
        {
            Bar = 0,
            Foo = 1,
            Lol = 4,
            [EnumMember(Value = "Ping")]
            Pong = 20,
            [EnumMember(Value = "Panser")]
            Mann,
            [EnumMember(Value = "Avfall")]
            Bio
        }

        [Test]
        [TestCase(FooEnum.Foo, ExpectedResult = BarEnum.Bar)]
        [TestCase(FooEnum.Bar, ExpectedResult = BarEnum.Foo)]
        [TestCase(FooEnum.Lol, ExpectedResult = BarEnum.Lol)]
        [TestCase(FooEnum.Ping, ExpectedResult = BarEnum.Pong)]
        [TestCase(FooEnum.Super, ExpectedResult = BarEnum.Mann)]
        public BarEnum ToEnum(FooEnum fooEnum)
        {
            return fooEnum.Map<BarEnum>();
        }

        [Test]
        [TestCase(FooEnum.Not)]
        [TestCase(FooEnum.Bio)] //Both FooEnum.Bio and BarEnum.Bio has EnumMember set, and therefore will only compare those and not the ToString representation.
        public void ToEnumShouldThrowException(FooEnum fooEnum)
        {
            Action action = () => fooEnum.Map<BarEnum>();

            action.ShouldThrow<ArgumentException>();
        }
    }
}