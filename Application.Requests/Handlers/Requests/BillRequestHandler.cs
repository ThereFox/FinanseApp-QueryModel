using Application.DTOs;
using Application.Stores;

namespace Application.Handlers.Requests;

public class BillRequestHandler
{
    private readonly IBillStore _store;

    public BillRequestHandler(IBillStore store)
    {
        _store = store;
    }
    
    public async Task<IEnumerable<BillPublicShortInfoDTO>> GetAllAsync()
    {
        var allBills = await _store.GetAll();

        return allBills;
    }
}