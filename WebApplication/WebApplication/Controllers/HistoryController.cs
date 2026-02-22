using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;
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
    public ActionResult<List<HistoryDto>> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<HistoryDto> GetById(int id)
    {
        return _service.GetById(id);
    }

    [HttpPost]
    public ActionResult<HistoryDto> AddItem(HistoryDto item)
    {
        return _service.AddItem(item);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateItem(int id, HistoryDto item)
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
