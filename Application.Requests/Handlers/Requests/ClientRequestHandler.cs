using Application.DTOs;
using Application.Stores;

namespace Application.Handlers.Requests;

public class ClientRequestHandler
{
    private readonly IClientStore _store;

    public ClientRequestHandler(IClientStore store)
    {
        _store = store;
    }

    public async Task<IEnumerable<ClientShortInfoDTO>> GetAllAsync()
    {
        return await _store.GetAll();
    }
    
}