using Application.DTOs;
using CSharpFunctionalExtensions;

namespace Persistense.Interfaces;

public interface IClientCacheStore
{
    public Task<Result<ClientShortInfoDTO>> GetByIdAsync(Guid clientId);
    public Task<Result> SaveById(Guid clientId, ClientShortInfoDTO client);
}