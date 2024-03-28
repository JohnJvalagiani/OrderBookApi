namespace OrderBook.API.QueryHandlers
{
    public interface IQueryHandler<TQuery, TResult>
    {
        TResult HandleAsync(TQuery query);
    }

}
