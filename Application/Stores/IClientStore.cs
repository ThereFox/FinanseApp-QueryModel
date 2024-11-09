using Application.DTOs;

namespace Application.Stores;

public interface IClientStore
{
    public Task<IEnumerable<ClientShortInfoDTO>> GetAll();
}