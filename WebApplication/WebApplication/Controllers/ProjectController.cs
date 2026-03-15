using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Service.Dto;
using Service.Interface;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController:ControllerBase
{
    private readonly IService<ProjectDto> _service;

    public ProjectController(IService<ProjectDto> service)
    {
        this._service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll()
    {
        return await _service.GetAll();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        return await _service.GetById(id);
    }
    
    [HttpPost]
    public async Task<ActionResult<ProjectDto>> AddItem(ProjectDto item)
    {
        return await _service.AddItem(item);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, ProjectDto item)
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