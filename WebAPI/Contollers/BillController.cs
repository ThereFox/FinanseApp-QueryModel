using Application.Handlers.Requests;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Contollers;

[Route("/api/bills")]
public class BillController : Controller
{
    private readonly BillRequestHandler _handler;

    public BillController(BillRequestHandler handler)
    {
        _handler = handler;
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAll()
    {
        var data = await _handler.GetAllAsync();
        return Json(data);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok();
    }
}