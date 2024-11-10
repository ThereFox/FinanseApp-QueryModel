using Application.Events.Abstractions;

namespace Application.Events.Realisation;

public class BillAmountChangeEvent : IDBStateChangeEvent
{
    public Guid BillId { get; set; }
    public decimal newAmount { get; set; }
}