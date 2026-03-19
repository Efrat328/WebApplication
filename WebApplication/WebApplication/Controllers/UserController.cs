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
        try
        {
            var result = await _service.AddItem(item);
            return Ok(result); // הכל טוב
        }
        catch (Exception ex)
        {
            // מחזיר 400 עם הודעה ידידותית למשתמש
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, UserDto item)
    {
        try
        {
             await _service.UpdateItem(id, item);
             var result = await _service.GetById(id);
             if (result == null)
                 return NotFound(new { message = "לא נמצא משתמש עם מזהה זה" });
             return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
         try
        {
            await _service.DeleteItem(id);
            return NoContent(); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}