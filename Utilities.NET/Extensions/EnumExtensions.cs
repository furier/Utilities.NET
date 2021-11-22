using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

// ReSharper disable MemberCanBePrivate.Global

namespace Utilities.NET.Extensions
{
    public enum MatchOn
    {
        /// <summary>
        ///     Either Number or Text must match, in that order.
        /// </summary>
        Either,

        /// <summary>
        ///     Both Number and Text must match.
        /// </summary>
        Both,

        /// <summary>
        ///     Matches only Text.
        /// </summary>
        Text,

        /// <summary>
        ///     Matches only Number.
        /// </summary>
        Number
    }

    public static class EnumExtensions
    {
        /// <summary>
        ///     Gets the type of the attribute of.
        /// </summary>
        /// <typeparam name="T"> The attribute type, eg: <see cref="EnumMemberAttribute"/>. </typeparam>
        /// <param name="source"> The enum value. </param>
        /// <returns> The value of the attribute of type <typeparamref name="T"/> on the passed in <paramref name="source"/>. </returns>
        public static T GetAttributeOfType<T>(this Enum source) where T : Attribute
        {
            var type = source.GetType();
            var memInfo = type.GetMember(source.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T) attributes[0] : null;
        }

        /// <summary>
        ///     Returns the value of the <see cref="DescriptionAttribute.Description"/> property on the
        ///     <see cref="DescriptionAttribute"/> if defined on the enum value, otherwise the <see cref="object.ToString"/>
        ///     representation of the enum value.
        /// </summary>
        /// <example>
        ///     public enum PublishStatusses
        ///     {
        ///     [Description("Not Completed")]
        ///     NotCompleted,
        ///     Completed,
        ///     Error
        ///     }
        ///     Console.WriteLine(PublishStatusses.NotCompleted.GetDescription()); // Console output: Not Completed.
        /// </example>
        /// <param name="enum">
        ///     The enum value to get the <see cref="DescriptionAttribute.Description"/> or
        ///     <see cref="object.ToString"/> representation from.
        /// </param>
        /// <returns>
        ///     Returns the value of the potensial <see cref="DescriptionAttribute.Description"/> property on the
        ///     <see cref="DescriptionAttribute"/> on the enum value.
        /// </returns>
        public static string GetDescription(this Enum @enum)
        {
            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var attribute = @enum.GetAttributeOfType<DescriptionAttribute>();
            //If we have no description attribute, just return the ToString of the enum
            return attribute != null
                ? attribute.Description
                : @enum.ToString();
        }

        /// <summary>
        ///     Maps an enum type to a different enum type.
        /// </summary>
        /// <typeparam name="T"> The enum type to map to. </typeparam>
        /// <param name="source"> The enum to map from. </param>
        /// <returns> The value as the new enum type. </returns>
        public static T Map<T>(this Enum source) => source.Map(MatchOn.Either, default(T), false, StringComparison.Ordinal);

        /// <summary>
        ///     Maps an enum type to a different enum type.
        /// </summary>
        /// <typeparam name="T"> The enum type to map to. </typeparam>
        /// <param name="source"> The enum to map from. </param>
        /// <param name="silent">
        ///     When set to true the default enum value is returned instead of throwing an exception
        ///     when mapping fails.
        /// </param>
        /// <returns> The value as the new enum type. </returns>
        public static T Map<T>(this Enum source, bool silent) => source.Map(MatchOn.Either, default(T), silent, StringComparison.Ordinal);

        /// <summary>
        ///     Maps an enum type to a different enum type.
        /// </summary>
        /// <typeparam name="T"> The enum type to map to. </typeparam>
        /// <param name="source"> The enum to map from. </param>
        /// <param name="matchOn"> Tells the method how to match the enums. </param>
        /// <param name="default"> The default value to be returned mapping fails and <paramref name="silent"/> is set to true. </param>
        /// <param name="silent">
        ///     When set to true <paramref name="default"/> value is returned instead of throwing an exception
        ///     when mapping fails.
        /// </param>
        /// <param name="stringComparison">
        ///     <see cref="StringComparison"/>
        /// </param>
        /// <returns> The value as the new enum type. </returns>
        public static T Map<T>(this Enum source, MatchOn matchOn, T @default, bool silent, StringComparison stringComparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var from = new EnumExt { Number = Convert.ToInt32(source), Text = Convert.ToString(source), AttrText = source.GetAttributeOfType<EnumMemberAttribute>()?.Value };
            var tos = Enum.GetValues(typeof(T)).Cast<Enum>().Select(x => new EnumExt { Number = Convert.ToInt32(x), Text = Convert.ToString(x), AttrText = x.GetAttributeOfType<EnumMemberAttribute>()?.Value }).ToList();

            switch (matchOn)
            {
                case MatchOn.Either:
                    foreach (var to in tos)
                        if (from.NumberEquals(to)) return to.Number.ToEnum<T>();
                    foreach (var to in tos)
                        if (from.TextEquals(to, stringComparison)) return to.Text.ToEnum<T>();

                    break;
                case MatchOn.Both:
                    foreach (var to in tos)
                        if (from.NumberEquals(to) && from.TextEquals(to, stringComparison)) return to.Number.ToEnum<T>();

                    break;
                case MatchOn.Text:
                    foreach (var to in tos)
                        if (from.TextEquals(to, stringComparison)) return to.Text.ToEnum<T>();

                    break;
                case MatchOn.Number:
                    foreach (var to in tos)
                        if (from.NumberEquals(to)) return to.Number.ToEnum<T>();

                    break;
            }

            return silent ? @default : throw new ArgumentException($"Unable to convert enum source type: {source.GetType().Name}, value: ({from.Number}){from.Text} to enum type: {typeof(T).Name}.");
        }

        private class EnumExt
        {
            public int Number { get; set; }
            public string Text { get; set; }
            public string AttrText { get; set; }

            public bool NumberEquals(EnumExt other)
            {
                return Number == other.Number;
            }

            public bool TextEquals(EnumExt other, StringComparison stringComparison)
            {
                if (AttrText.IsNotNullOrWhiteSpace() && other.AttrText.IsNotNullOrWhiteSpace())
                    return string.Equals(AttrText, other.AttrText, stringComparison);

                if (other.AttrText.IsNotNullOrWhiteSpace())
                    return string.Equals(Text, other.AttrText, stringComparison);

                if (AttrText.IsNotNullOrWhiteSpace())
                    return string.Equals(AttrText, other.Text, stringComparison);

                return string.Equals(Text, other.Text, stringComparison);
            }
        }
    }
}