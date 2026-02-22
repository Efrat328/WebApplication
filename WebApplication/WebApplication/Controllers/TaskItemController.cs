using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class TaskItemController:ControllerBase
{
    private readonly IService<TaskItemDto> _service;

    public TaskItemController(IService<TaskItemDto> service)
    {
        this._service = service;
    }
    
    [HttpGet]
    public ActionResult<List<TaskItemDto>> GetAll()
    {
        return _service.GetAll();
    }
    
    [HttpGet("{id}")]
    public ActionResult<TaskItemDto> GetById(int id)
    {
        return _service.GetById(id);
    }
    
    [HttpPost]
    public ActionResult<TaskItemDto> AddItem(TaskItemDto item)
    {
        return _service.AddItem(item);
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateItem(int id, TaskItemDto item)
    {
        _service.UpdateItem(id, item);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteItem(int id)
    {
        _service.DeleteItem(id);
        return NoContent();
    }
}