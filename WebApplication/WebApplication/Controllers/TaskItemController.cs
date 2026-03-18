using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskItemController:ControllerBase
{
    private readonly IService<TaskItemDto> _service;

    public TaskItemController(IService<TaskItemDto> service)
    {
        this._service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<TaskItemDto>>> GetAll()
    {
        return await _service.GetAll();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemDto>> GetById(int id)
    {
        return await _service.GetById(id);
    }
    
    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> AddItem(TaskItemDto item)
    {
        return await _service.AddItem(item);
    }
    
    [HttpPut("{id}")]
    
    public async Task<ActionResult<TaskItemDto>> UpdateItem(int id, TaskItemDto item)
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