using Application.DTOs;
using Application.Stores;
using CSharpFunctionalExtensions;

namespace Persistense.Stores;

public class BillStore : IBillStore
{
    public Task<IList<BillShortInfoDTO>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IList<BillShortInfoDTO>> GetWithBalanse()
    {
        throw new NotImplementedException();
    }

    public Task<Result<BillShortInfoDTO>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}