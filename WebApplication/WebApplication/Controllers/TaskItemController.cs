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
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<TaskItemDto>> AddItem(TaskItemDto item)
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

    public async Task<ActionResult<TaskItemDto>> UpdateItem(int id, TaskItemDto item)
    {
        try
        {
            await _service.UpdateItem(id, item);
            var result = await _service.GetById(id);
            if (result == null)
                return NotFound(new { message = "לא נמצאה משימה עם מזהה זה" });

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