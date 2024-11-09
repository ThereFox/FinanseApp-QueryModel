using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Contollers;

[Route("/api/bills")]
public class BillController : Controller
{
    [HttpGet]
    [Route("")]
    public IActionResult GetAll()
    {
        
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        
    }
}