using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IMagicFactory<T>
    {
        T Create();

    }
    public interface IMagicFactory<T, T1>
    {
        T Create(T1 arg);

    }
    public interface IMagicFactory<T, T1, T2>
    {
        T Create(T1 arg1, T2 arg2);

    }
    public interface IMagicFactory<T, T1, T2, T3>
    {
        T Create(T1 arg1, T2 arg2, T3 arg3);

    }
    public interface IMagicFactory<T, T1, T2, T3, T4>
    {
        T Create(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    }
}
