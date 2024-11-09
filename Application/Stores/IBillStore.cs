using Application.DTOs;
using CSharpFunctionalExtensions;

namespace Application.Stores;

public interface IBillStore
{
    public Task<IList<BillShortInfoDTO>> GetAll();
    public Task<IList<BillShortInfoDTO>> GetWithBalanse();
    public Task<Result<BillShortInfoDTO>> GetById(Guid id);
}