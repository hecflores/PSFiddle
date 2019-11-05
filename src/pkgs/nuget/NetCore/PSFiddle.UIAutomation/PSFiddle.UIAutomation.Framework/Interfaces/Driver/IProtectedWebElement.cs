using MC.Track.TestSuite.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Driver
{
    public interface IProtectedWebElement: IHoverable
    {
        String Name { get; }
        String GetText();
        void ClickIt();
        void VerifyElementHasText();
        void VerifyElementIsVisable();
        void VerifyElementIsHidden();
        void VerifyElementIsNotEnabled();
        void VerifyFAIcon(FAIcons iconType);
        /// <summary>
        /// *Warning* Using any methods after this point are not expected to be reliable and is prone to errors
        /// </summary>
        /// <returns></returns>
        IWebElementWrapper Unprotected();

    }
}
