using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IExpectedVsActualService
    {
        String Name();
        String Description();
        bool StrictVerifications();
        void StrictVerifications(bool strict);
        bool AllowExpectedOverwrites();
        void AllowExpectedOverwrites(bool overwriteOrNot);
        bool AllowActualsOverwrites();
        void AllowActualsOverwrites(bool overwriteOrNot);
        bool AllowDataOverwrites();
        void AllowDataOverwrites(bool overwriteOrNot);
        void Expected(String Name, Object Value);
        Object Expected(String Name);
        bool HasExpected(String Name);
        void Actual(String Name, Object Value);
        Object Actual(String Name);
        bool HasActual(String Name);
        void Data(String Name, Object Value);
        Object Data(String Name);
        bool HasData(String Name);
        void VerifyExpectations();
    }
}
