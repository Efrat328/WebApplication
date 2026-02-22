using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;

[ApiController]
[Route("api/[controller]")]
public class ProjectController:ControllerBase
{
    private readonly IService<ProjectDto> _service;

    public ProjectController(IService<ProjectDto> service)
    {
        this._service = service;
    }
    
    [HttpGet]
    public ActionResult<List<ProjectDto>> GetAll()
    {
        return _service.GetAll();
    }
    
    [HttpGet("{id}")]
    public ActionResult<ProjectDto> GetById(int id)
    {
        return _service.GetById(id);
    }
    
    [HttpPost]
    public ActionResult<ProjectDto> AddItem(ProjectDto item)
    {
        return _service.AddItem(item);
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateItem(int id, ProjectDto item)
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