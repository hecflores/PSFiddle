using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IDynamicUserFactoryService
    {
        DynamicUserType TakeUser();
        void FreeUser(String email);
        void Initialize();

    }
}
