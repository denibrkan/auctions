namespace SearchService.RequestHelpers
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Results { get; set; }
        public int PageCount { get; set; }
        public long TotalCount { get; set; }

        public PagedResult(ValueTuple<IReadOnlyList<T>, long, int> result)
        {
            Results = result.Item1;
            TotalCount = result.Item2;
            PageCount = result.Item3;
        }
    }
}
