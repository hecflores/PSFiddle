using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared;

namespace MC.Track.TestSuite.Toolkit.Dependencies.Builders.Shared
{
    public class BaseListBuilder<T, E> : BaseBuilder<List<T>, E>, IBaseListBuilder<T, E> where E : IBaseListBuilder<T, E>
    {
        public E Repeat(int numberOfTimes, Action<E> callback)
        {
            for (int i = 0; i < numberOfTimes; i++)
            {
                callback((E)((IBaseListBuilder<T, E>)this));
            }
            return (E)((IBaseListBuilder<T, E>)this);
        }
        public T BuildSingle()
        {
            return this.Build()[0];
        }
    }
}
