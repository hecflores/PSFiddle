using PSFiddle.UIAutomation.Framework.Exceptions;
using PSFiddle.UIAutomation.Framework.Extensions;
using MC.Track.TestSuite.Interfaces.Pages.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static class MetisAssert
    {
        /// <summary>
        /// Ares the equal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Name">The name.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <exception cref="PSFiddle.UIAutomation.Framework.Exceptions.MetisAssertFailException">Expected '{Name}' to be '{expected}' but was found as '{actual}</exception>
        public static void AreEqual<T>(String Name, T expected, T actual)
        {
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch(Exception e)
            {
                throw new MetisAssertFailException($"Expected '{Name}' to be '{expected}' but was found as '{actual}'", e);
            }
        }

        public static class POMAssert
        {
            /// <summary>
            /// Determines whether the specified test it is true.
            /// </summary>
            /// <param name="testIt">if set to <c>true</c> [test it].</param>
            /// <param name="Page">The page.</param>
            /// <param name="Message">The message.</param>
            /// <exception cref="PageObjectException"></exception>
            public static void IsTrue(bool testIt, String Page, String Message)
            {
                try
                {
                    Assert.IsTrue(testIt, Message);
                }
                catch(Exception e)
                {
                    throw new PageObjectException(Page, Message);
                }
            }

            /// <summary>
            /// Determines whether the specified test it is true.
            /// </summary>
            /// <param name="testIt">if set to <c>true</c> [test it].</param>
            /// <param name="PageType">Type of the page.</param>
            /// <param name="Message">The message.</param>
            public static void IsTrue(bool testIt, Type PageType, String Message)
            {
                IsTrue(testIt, PageType.Name.UndoCamelCase(), Message);
            }

            /// <summary>
            /// Determines whether the specified test it is true.
            /// </summary>
            /// <param name="testIt">if set to <c>true</c> [test it].</param>
            /// <param name="page">The page.</param>
            /// <param name="Message">The message.</param>
            public static void IsTrue(bool testIt, IPageBase page, String Message)
            {
                IsTrue(testIt, page.GetType(), Message);
            }

            /// <summary>
            /// Determines whether the specified test it is false.
            /// </summary>
            /// <param name="testIt">if set to <c>true</c> [test it].</param>
            /// <param name="Page">The page.</param>
            /// <param name="Message">The message.</param>
            /// <exception cref="PageObjectException"></exception>
            public static void IsFalse(bool testIt, String Page, String Message)
            {
                try
                {
                    Assert.IsFalse(testIt, Message);
                }
                catch (Exception e)
                {
                    throw new PageObjectException(Page, Message);
                }
            }

            /// <summary>
            /// Determines whether the specified test it is false.
            /// </summary>
            /// <param name="testIt">if set to <c>true</c> [test it].</param>
            /// <param name="PageType">Type of the page.</param>
            /// <param name="Message">The message.</param>
            public static void IsFalse(bool testIt, Type PageType, String Message)
            {
                IsFalse(testIt, PageType.Name.UndoCamelCase(), Message);
            }

            /// <summary>
            /// Determines whether the specified test it is false.
            /// </summary>
            /// <param name="testIt">if set to <c>true</c> [test it].</param>
            /// <param name="page">The page.</param>
            /// <param name="Message">The message.</param>
            public static void IsFalse(bool testIt, IPageBase page, String Message)
            {
                IsFalse(testIt, page.GetType(), Message);
            }
        }
    }
}
