using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;
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
    public ActionResult<List<SubTaskDto>> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<SubTaskDto> GetById(int id)
    {
        return _service.GetById(id);
    }

    [HttpPost]
    public ActionResult<SubTaskDto> AddItem(SubTaskDto item)
    {
        return _service.AddItem(item);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateItem(int id, SubTaskDto item)
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