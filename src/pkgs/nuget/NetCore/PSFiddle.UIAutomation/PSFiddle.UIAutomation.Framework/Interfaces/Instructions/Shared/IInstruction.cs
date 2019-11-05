using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Instructions.Shared
{
    public interface IInstruction<T> where T: IInstruction<T>
    {
        T Actual(String Name, String Value);
        T Actual(String Name, bool Value);
        T Data(string Name, bool Value);
        T Data(String Name, String Value);
        T Expected(String Name, String Value);
        T Expected(string Name, bool Value);

        String Data(String Name);
        String Expected(String Name);
        String Actual(String Name);

        IInstructions All();
    }
}
