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
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<SubTaskDto>> AddItem(SubTaskDto item)
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
    public async Task<ActionResult<SubTaskDto>> UpdateItem(int id, SubTaskDto item)
    {
        try
        {
            await _service.UpdateItem(id, item);          
            var result = await _service.GetById(id);      
            if (result == null)
                return NotFound(new { message = "לא נמצאה תת-משימה עם מזהה זה" });

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
            return NoContent(); // או Ok עם הודעה
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}