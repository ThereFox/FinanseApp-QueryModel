using Application.Events.Abstractions;

namespace Application.Events.Realisation;

public class BillAmountChanged : IEvent
{
    public Guid BillId { get; set; }
    public decimal newAmount { get; set; }
}