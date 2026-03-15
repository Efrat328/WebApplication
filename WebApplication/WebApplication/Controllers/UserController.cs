using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController:ControllerBase
{
    private readonly IService<UserDto> _service;

    public UserController(IService<UserDto> service)
    {
        this._service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        return await _service.GetAll();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        return await _service.GetById(id);
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddItem(UserDto item)
    {
        return await _service.AddItem(item);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, UserDto item)
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