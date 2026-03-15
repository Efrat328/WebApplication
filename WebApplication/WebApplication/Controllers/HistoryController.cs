using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly IService<HistoryDto> _service;

    public HistoryController(IService<HistoryDto> service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<HistoryDto>>> GetAll()
    {
        return await _service.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HistoryDto>> GetById(int id)
    {
        return await _service.GetById(id);
    }

    [HttpPost]
    public async Task<ActionResult<HistoryDto>> AddItem(HistoryDto item)
    {
        return await _service.AddItem(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, HistoryDto item)
    {
        await _service.UpdateItem(id, item);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        await _service.DeleteItem(id);
        return NoContent();
    }
}
