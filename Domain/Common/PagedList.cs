namespace api.Domain.Common
{
    public class PagedList<T>
    {
        public PagedList(IList<T> list, int count)
        {
            List = list;
            TotalCount = count;
        }
        public IList<T> List { get; protected set; }
        public int TotalCount { get; protected set; }
    }
}
