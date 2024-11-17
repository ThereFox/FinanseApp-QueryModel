using Application.DTOs;
using CSharpFunctionalExtensions;

namespace Persistense.Interfaces;

public interface IBillCacheStore
{
    public Task<Result<List<BillShortInfoDTO>>> GetAllByOwner(Guid OwnerId);
    public Task<Result<BillShortInfoDTO>> GetById(Guid id);
    
    public Task<Result> SaveById(Guid id, BillShortInfoDTO bill);
    public Task<Result> SaveByOwnerId(Guid OwnerId, List<BillShortInfoDTO> bills);
}