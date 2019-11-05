using System;

namespace MC.Track.TestSuite.Interfaces.Dependencies.Builders.Shared
{
    public interface IBaseBuilder<T1Output, T2ThisType> where T2ThisType : IBaseBuilder<T1Output, T2ThisType>
    {
        T2ThisType AddBuildStep(string name, Func<T1Output, T1Output> action);
        T1Output Build();
        T2ThisType InsertBuildStep(int pos, string name, Func<T1Output, T1Output> action);
        T2ThisType Out(Action<T1Output> callback);
    }
}