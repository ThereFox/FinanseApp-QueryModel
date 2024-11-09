using Application.DTOs;
using CSharpFunctionalExtensions;

namespace Application.Stores;

public interface IBillStore
{
    public Task<IEnumerable<BillPublicShortInfoDTO>> GetAll();
    public Task<IEnumerable<BillPublicShortInfoDTO>> GetWithBalanse();
    public Task<Result<BillPublicShortInfoDTO>> GetById(Guid id);
}