using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared
{
    public interface IBaseListBuilder<T1Output, T2ThisType> : IBaseBuilder<List<T1Output>, T2ThisType>
                where T2ThisType : IBaseBuilder<List<T1Output>, T2ThisType>
    {
        T2ThisType Repeat(int numberOfTimes, Action<T2ThisType> callback);
        T1Output BuildSingle();
    }
}
