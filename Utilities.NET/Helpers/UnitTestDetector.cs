using System;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global

namespace Utilities.NET.Helpers
{
    /// <summary>
    ///     Detects if we are running inside a unit test.
    /// </summary>
    /// <remarks>
    ///     Currently Supporting:
    ///     - NUnit
    ///     - MSTest V1.0 (Microsoft.VisualStudio.QualityTools.UnitTestFramework)
    ///     - MSTest V2.0 (Microsoft.VisualStudio.TestPlatform.TestFramework)
    /// </remarks>
    public static class UnitTestDetector
    {
        static UnitTestDetector()
        {
            IsInUnitTest = AppDomain.CurrentDomain.GetAssemblies()
                .Any(
                    a => a.FullName.StartsWith("nunit.framework", StringComparison.OrdinalIgnoreCase) ||
                        a.FullName.StartsWith("microsoft.visualstudio.qualitytools.unittestframework", StringComparison.OrdinalIgnoreCase) ||
                        a.FullName.StartsWith("microsoft.visualstudio.testplatform.testframework", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Indicating whether we are running inside a unit test or not.
        /// </summary>
        /// <value>
        ///     true if we are running inside a unit test, otherwise false.
        /// </value>
        public static bool IsInUnitTest { get; }
    }
}