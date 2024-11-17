using Application.DTOs;
using CSharpFunctionalExtensions;

namespace Application.Stores;

public interface IBillStore
{
    public Task<IEnumerable<BillPublicShortInfoDTO>> GetAll();
    public Task<Result<List<BillShortInfoDTO>>> GetAllByOwner(Guid OwnerId);
    public Task<IEnumerable<BillShortInfoDTO>> GetWithBalanse();
    public Task<Result<BillShortInfoDTO>> GetById(Guid id);
}