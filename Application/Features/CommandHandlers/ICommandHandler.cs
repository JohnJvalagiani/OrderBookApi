namespace OrderBook.Application.Features.CommandHandlers
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}
