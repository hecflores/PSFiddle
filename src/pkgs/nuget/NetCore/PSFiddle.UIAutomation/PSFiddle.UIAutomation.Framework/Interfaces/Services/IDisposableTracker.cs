using MC.Track.TestSuite.Model.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Interfaces.Services
{
    public interface IDisposableTracker  : IDisposable
    {
        /// <summary>
        /// Gets all the dependencies of the type given. Meaning, all the types that need to be disposed before the type given
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<Type> GetDependenciesTypes(Type type);
        /// <summary>
        /// Adds a dependency on disposing
        /// </summary>
        /// <param name="depends">The thing that is depends on something</param>
        /// <param name="dependsOn">The thing that it depends on</param>
        void AddDependency(Type dependsOnSomething, Type theDependent);
        void Add(DisposableResourceType obj);
        List<T> DestroyType<T>();
        List<Type> DestroyType(Type type);
    }
}
