using dto;
using entities;
using exceptionError;
using interfaces;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace controller;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("rateLimitRquest")]
public class TasksController : ControllerBase
{
    private readonly IUnitOfWork _uof;

    public TasksController(IUnitOfWork uof)
    {
        _uof = uof;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(TaskCreateResDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TaskCreateResDTO>> Create([FromBody] TaskEntity taskReq)
    {
        var userId = _uof.TaskRepository.GetUserId(User);
        try
        {
            var taskEntity = _uof.TaskRepository.GetTaskEntityDTO(taskReq, userId);
            var addTaskEntity = await _uof.TaskRepository.AddAsync(taskEntity);
            await _uof.SaveChangesAsync();
            var task = _uof.TaskRepository.GetTaskCreateResDTO(addTaskEntity);
            return Created("/Task", task);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex);
        }
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(TaskListResDTO<TaskByUserIdDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TaskByUserIdDTO>>> GetTaskByUser(
        [FromQuery] PageListDTO pageDto,
        [FromQuery] TaskOrderQueryDTO orderDTO
        )
    {
        if (pageDto.PageSize <= 0 || pageDto.PageNumber <= 0)
            return BadRequest(new ResponseDTO { Status = "Error", Message = "PageSize and PagNumber in Query do not have a valid value" });

        var userId = _uof.TaskRepository.GetUserId(User);

        try
        {
            var totalTasksList = await _uof.TaskRepository.GetTotalTaskByUserIDListAsync(userId, orderDTO, pageDto);
            if (totalTasksList == null)
                return NotFound(new ResponseDTO { Status = "Error", Message = "List don't found." });

            var allList = await _uof.TaskRepository.GetAllListAsync(userId);
            var pagination = _uof.TaskRepository.PaginationRes(allList.Count(), pageDto);

            var paginationDTOResult = _uof.TaskRepository.GetResLisDTO(
                pageDto, pagination.TotalPages, pagination.HasNextPage, totalTasksList);

            return Ok(paginationDTOResult);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex);
        }


    }

    [HttpPut("{taskId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(TaskByUserIdDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TaskUptadeDTO>> TaskUpdateByUserID([FromBody] TaskUptadeDTO dto, string taskId)
    {
        var userId = _uof.TaskRepository.GetUserId(User);
        try
        {
            var taskByUser = await _uof.TaskRepository.GetTaskByUserIdAsync(taskId, userId);
            if (taskByUser == null)
                return NotFound(new ResponseDTO { Status = "Error", Message = "Task not found." });

            var taskTableUpdate = _uof.TaskRepository.GetTaskUpdateTable(taskByUser, dto);

            _uof.TaskRepository.Update(taskTableUpdate);
            await _uof.SaveChangesAsync();

            var taskUptadeModel = _uof.TaskRepository.GetTaskUptadeDTO(taskTableUpdate);
            return Ok(taskUptadeModel);

        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex);
        }
    }

    [HttpDelete("{taskId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteTaskByUserId(string taskId)
    {
        var userId = _uof.TaskRepository.GetUserId(User);
        try
        {
            var taskByUser = await _uof.TaskRepository.GetTaskByUserIdAsync(taskId, userId);
            if (taskByUser == null)
                return NotFound(new ResponseDTO { Status = "Error", Message = "Task not found." });


            _uof.TaskRepository.Remove(taskByUser);
            await _uof.SaveChangesAsync();

            return Ok(new ResponseDTO { Status = "Success.", Message = "Task deleted successfully." });
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex);
        }
    }

    [HttpDelete("delete-list")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteMultipleTasksByUserId([FromBody] TaskDeleteListDTO model)
    {
        var userId = _uof.TaskRepository.GetUserId(User);
        try
        {
            var tasksToDelete = await _uof.TaskRepository.GetTaskListToDeleteAsync(model, userId);
            if (tasksToDelete == null)
                return NotFound(new ResponseDTO { Status = "Error", Message = "No tasks found or none belong to the user." });


            _uof.TaskRepository.RemoveRange(tasksToDelete);
            await _uof.SaveChangesAsync();

            return Ok(new ResponseDTO { Status = "Success", Message = "Successfully deleted Task List." });
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex);
        }
    }


}
