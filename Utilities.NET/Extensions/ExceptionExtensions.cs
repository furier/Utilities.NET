using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global

namespace Utilities.NET.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Recursively aggregates all exceptions and their inner exceptions to a flat list of exception messages.
        /// </summary>
        public static IEnumerable<string> GetMessages(this Exception source) => source.Aggregate().Select(x => x.Message);

        /// <summary>
        ///     Recursively aggregates all exceptions and their inner exceptions to a flat list of exceptions.
        /// </summary>
        public static IEnumerable<Exception> Aggregate(this Exception source) => source.InnerException?.Aggregate().Concat(new[] { source }) ?? new[] { source };
    }
}