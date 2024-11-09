using Application.Handlers.Requests;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Contollers;

[Route("api/clients/")]
[Controller]
public class ClientController : Controller
{
    private readonly ClientRequestHandler _handler;

    public ClientController(ClientRequestHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _handler.GetAllAsync();
        return Json(result);
    }
    
}