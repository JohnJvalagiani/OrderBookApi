namespace OrderBook.API.QueryHandlers
{
    public interface IQueryHandler<TQuery, TResult>
    {
        TResult Handle(TQuery query);
    }

}
