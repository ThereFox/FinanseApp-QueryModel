using Application.DTOs;
using Application.Stores;

namespace Application.Handlers.Requests;

public class BillRequestHandler
{
    private readonly IBillStore _store;
    
    public async Task<IEnumerable<BillShortInfoDTO>> GetAll()
    {
        var allBills = await _store.GetAll();

        return allBills;
    }
    public
}