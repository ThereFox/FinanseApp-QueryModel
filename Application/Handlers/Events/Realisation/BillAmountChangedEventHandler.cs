using Application.Events.Realisation;
using Application.Handlers.Abstcations;

namespace Application.Handlers.Realisation;

public class BillAmountChangedEventHandler : IEventHadler<BillAmountChanged>
{
    public void Handle(BillAmountChanged @event)
    {
    }
}