namespace MC.Track.TestSuite.Interfaces.Pages.Shared
{
    public interface IPaginationControls<T>: IScopedPageUI where T : class, IPageBase
    {
        T NextPage<T2>() where T2 : class, T;
        T PreviousPage<T2>() where T2: class, T;
        T NthPage<T2>(int pageNumber) where T2: class, T;
    }
}