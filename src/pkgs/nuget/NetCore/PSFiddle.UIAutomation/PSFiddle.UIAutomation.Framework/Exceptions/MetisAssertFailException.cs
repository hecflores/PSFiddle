using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSFiddle.UIAutomation.Framework.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException" />
    public class MetisAssertFailException : AssertFailedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetisAssertFailException"/> class.
        /// </summary>
        public MetisAssertFailException() :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetisAssertFailException"/> class.
        /// </summary>
        /// <param name="msg">The message.</param>
        public MetisAssertFailException(String msg) : base(msg)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetisAssertFailException"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="inner">The inner.</param>
        public MetisAssertFailException(String msg, Exception inner) : base(msg, inner)
        {

        }
    }
}
