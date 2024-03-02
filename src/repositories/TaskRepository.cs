using dataContext;
using dto;
using entities;
using interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace repositories;

public class TaskRepository : GlobalRepository<TaskEntity>, ITaskRepository
{
    public TaskRepository(DataContext context) : base(context)
    {
    }
    public TaskEntity GetTaskEntityDTO(TaskEntity task, string userId)
    {
        TaskEntity taskEntity = new()
        {
            Id = Guid.NewGuid().ToString(),
            Title = task.Title,
            Description = task.Description,
            DateCreate = DateTime.Now,
            DateUpgrade = DateTime.Now,
            UserId = userId
        };
        return taskEntity;
    }

    public TaskCreateResDTO GetTaskCreateResDTO(TaskEntity task)
    {
        TaskCreateResDTO taskRes = new()
        {
            Title = task.Title!,
            Description = task.Description!,
            DateCreate = DateTime.Now
        };
        return taskRes;
    }

    public async Task<IEnumerable<TaskEntity>> GetAllListAsync(string userId)
    {
        var userTasks = await _context.Tasks.Where(task => task.UserId.ToString() == userId)
                                            .AsNoTracking()
                                            .ToListAsync();
        return userTasks;
    }


    public async Task<IEnumerable<TaskByUserIdDTO>> GetTotalTaskByUserIDListAsync(
        string userId, TaskOrderQueryDTO orderDTO, PageListDTO pageDto
        )
    {
        if (orderDTO.OrderBy == TaskOrderQueryEnum.dateCreate && orderDTO.Reverse == false && orderDTO.Search.IsNullOrEmpty())
        {

            var userTasks = await _context.Tasks.Where(task => task.UserId.ToString() == userId)
                                                .Select(task => new TaskByUserIdDTO
                                                {
                                                    Id = task.Id.ToString(),
                                                    Title = task.Title!,
                                                    Description = task.Description!,
                                                    DateCreate = task.DateCreate,
                                                    DateUpgrade = task.DateUpgrade
                                                })
                                                .OrderByDescending(order => order.DateCreate)
                                                .Skip((pageDto.PageNumber - 1) * pageDto.PageSize)
                                                .Take(pageDto.PageSize)
                                                .AsNoTracking()
                                                .ToListAsync();
            return userTasks;
        }
        else if (orderDTO.OrderBy == TaskOrderQueryEnum.dateCreate && orderDTO.Reverse == true && orderDTO.Search.IsNullOrEmpty())
        {

            var userTasks = await _context.Tasks.Where(task => task.UserId.ToString() == userId)
                                                .Select(task => new TaskByUserIdDTO
                                                {
                                                    Id = task.Id.ToString(),
                                                    Title = task.Title!,
                                                    Description = task.Description!,
                                                    DateCreate = task.DateCreate,
                                                    DateUpgrade = task.DateUpgrade
                                                })
                                                .OrderBy(order => order.DateCreate)
                                                .Skip((pageDto.PageNumber - 1) * pageDto.PageSize)
                                                .Take(pageDto.PageSize)
                                                .AsNoTracking()
                                                .ToListAsync();
            return userTasks;
        }
        else if (orderDTO.OrderBy == TaskOrderQueryEnum.dateUpgrade && orderDTO.Reverse == false && orderDTO.Search.IsNullOrEmpty())
        {
            var userTasks = await _context.Tasks.Where(task => task.UserId.ToString() == userId)
                                                .Select(task => new TaskByUserIdDTO
                                                {
                                                    Id = task.Id.ToString(),
                                                    Title = task.Title!,
                                                    Description = task.Description!,
                                                    DateCreate = task.DateCreate,
                                                    DateUpgrade = task.DateUpgrade
                                                })
                                            .OrderByDescending(order => order.DateUpgrade)
                                            .Skip((pageDto.PageNumber - 1) * pageDto.PageSize)
                                            .Take(pageDto.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();
            return userTasks;
        }
        else if (orderDTO.OrderBy == TaskOrderQueryEnum.dateUpgrade && orderDTO.Reverse == true && orderDTO.Search.IsNullOrEmpty())
        {
            var userTasks = await _context.Tasks.Where(task => task.UserId.ToString() == userId)
                                                .Select(task => new TaskByUserIdDTO
                                                {
                                                    Id = task.Id.ToString(),
                                                    Title = task.Title!,
                                                    Description = task.Description!,
                                                    DateCreate = task.DateCreate,
                                                    DateUpgrade = task.DateUpgrade
                                                })
                                            .OrderBy(order => order.DateUpgrade)
                                            .Skip((pageDto.PageNumber - 1) * pageDto.PageSize)
                                            .Take(pageDto.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();
            return userTasks;
        }
        else if (orderDTO.Search!.Length > 0 && orderDTO.Reverse == false)
        {
            var userTasks = await _context.Tasks.Where(task => task.UserId.ToString() == userId && task.Title!.Contains(orderDTO.Search!))
                                                .Select(task => new TaskByUserIdDTO
                                                {
                                                    Id = task.Id.ToString(),
                                                    Title = task.Title!,
                                                    Description = task.Description!,
                                                    DateCreate = task.DateCreate,
                                                    DateUpgrade = task.DateUpgrade
                                                })
                                                .OrderByDescending(order => order.DateUpgrade)
                                                .Skip((pageDto.PageNumber - 1) * pageDto.PageSize)
                                                .Take(pageDto.PageSize)
                                                .AsNoTracking()
                                                .ToListAsync();
            return userTasks;
        }
        else
        {
            var userTasks = await _context.Tasks.Where(task => task.UserId.ToString() == userId && task.Title!.Contains(orderDTO.Search!))
                                                .Select(task => new TaskByUserIdDTO
                                                {
                                                    Id = task.Id.ToString(),
                                                    Title = task.Title!,
                                                    Description = task.Description!,
                                                    DateCreate = task.DateCreate,
                                                    DateUpgrade = task.DateUpgrade
                                                })
                                                .OrderBy(order => order.DateUpgrade)
                                                .Skip((pageDto.PageNumber - 1) * pageDto.PageSize)
                                                .Take(pageDto.PageSize)
                                                .AsNoTracking()
                                                .ToListAsync();
            return userTasks;
        }
    }

    public async Task<TaskEntity?> GetTaskByUserIdAsync(string taskId, string userId)
    {
        return await _context.Tasks.FirstOrDefaultAsync(task => task.Id == taskId && task.UserId == userId);
    }

    public TaskEntity GetTaskUpdateTable(TaskEntity taskEntity, TaskUptadeDTO model)
    {
        taskEntity.Title = model.Title;
        taskEntity.Description = model.Description;
        taskEntity.DateUpgrade = DateTime.Now;
        return taskEntity;
    }

    public TaskUptadeResDTO GetTaskUptadeDTO(TaskEntity task)
    {
        TaskUptadeResDTO taskUptade = new()
        {
            Title = task.Title!,
            Description = task.Description!,
            DateUpgrade = DateTime.Now,
            DateCreate = task.DateCreate
        };
        return taskUptade;
    }

    public async Task<IEnumerable<TaskEntity>> GetTaskListToDeleteAsync(TaskDeleteListDTO model, string userId)
    {
        var tasksToDelete = await _context.Tasks
                .Where(task => model.TaskIds.Contains(task.Id) && task.UserId == userId)
                .ToListAsync();
        return tasksToDelete;
    }

    public TaskListResDTO<TaskByUserIdDTO> GetResLisDTO(
        PageListDTO pageDTO, int totalPages, bool hasNextPage, IEnumerable<TaskByUserIdDTO> tasksTotalList)
    {

        var paginationResult = new TaskListResDTO<TaskByUserIdDTO>
        {
            PageSize = pageDTO.PageSize,
            PageNumber = pageDTO.PageNumber,
            TotalPages = totalPages,
            HasNextPage = hasNextPage,
            List = tasksTotalList,
        };

        return paginationResult;

    }
}
