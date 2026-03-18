using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SubTaskController : ControllerBase
{
    private readonly IService<SubTaskDto> _service;

    public SubTaskController(IService<SubTaskDto> service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<SubTaskDto>>> GetAll()
    {
        return await _service.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubTaskDto>> GetById(int id)
    {
        return await _service.GetById(id);
    }

    [HttpPost]
    public async Task<ActionResult<SubTaskDto>> AddItem(SubTaskDto item)
    {
        return await _service.AddItem(item);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SubTaskDto>> UpdateItem(int id, SubTaskDto item)
    {
        await _service.UpdateItem(id, item);
        return Ok(await _service.GetById(id));  // ✅
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        await _service.DeleteItem(id);
        return NoContent();
    }
}