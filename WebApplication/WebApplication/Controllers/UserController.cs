using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;


[ApiController]
[Route("api/[controller]")]
public class UserController:ControllerBase
{
    private readonly IService<UserDto> _service;

    public UserController(IService<UserDto> service)
    {
        this._service = service;
    }
    
    [HttpGet]
    public ActionResult<List<UserDto>> GetAll()
    {
        return _service.GetAll();
    }
    
    [HttpGet("{id}")]
    public ActionResult<UserDto> GetById(int id)
    {
        return _service.GetById(id);
    }
    
    [HttpPost]
    public ActionResult<UserDto> AddItem(UserDto item)
    {
        return _service.AddItem(item);
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateItem(int id, UserDto item)
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