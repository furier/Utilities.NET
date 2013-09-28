#region File Header

// //////////////////////////////////////////////////////
// /// File: UnitTestDetector.cs
// /// Author: Sander Struijk
// /// Date: 2013-09-28 15:07
// //////////////////////////////////////////////////////

#endregion

#region Using Directives

using System;
using System.Linq;

#endregion

namespace Utilities.NET.Tests
{
    /// <summary>   Detects if we are running inside a unit test. </summary>
    /// <remarks>   Sander Struijk, 24.09.2013. </remarks>
    public static class UnitTestDetector
    {
        /// <summary>   Static constructor. </summary>
        /// <remarks>   Sander Struijk, 24.09.2013. </remarks>
        static UnitTestDetector()
        {
            const string testAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
            IsInUnitTest = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.StartsWith(testAssemblyName));
        }

        /// <summary>   Gets or sets a value indicating whether we are running inside a unit test. </summary>
        /// <value> true if we are running inside a unit test, false if not. </value>
        public static bool IsInUnitTest { get; private set; }
    }
}