using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IStateManagment
    {
        T Get<T>(String Name);
        Object Get(String Name);
        bool Has<T>(string Name);
        void Set<T>(String name, T obj);
        bool Has(String Name);
    }
}
