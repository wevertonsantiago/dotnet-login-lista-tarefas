using dto;
using entities;

namespace interfaces;

public interface ITaskRepository : IGlobalRepository<TaskEntity>
{
    TaskEntity GetTaskEntityDTO(TaskEntity task, string userId);
    TaskCreateResDTO GetTaskCreateResDTO(TaskEntity task);
    Task<IEnumerable<TaskEntity>> GetAllListAsync(string userId);
    Task<IEnumerable<TaskByUserIdDTO>> GetTotalTaskByUserIDListAsync(string userId, TaskOrderQueryDTO orderDTO, PageListDTO pageDto);
    Task<TaskEntity?> GetTaskByUserIdAsync(string taskId, string userId);
    TaskEntity GetTaskUpdateTable(TaskEntity taskEntity, TaskUptadeDTO dto);
    TaskUptadeResDTO GetTaskUptadeDTO(TaskEntity taskEntity);
    Task<IEnumerable<TaskEntity>> GetTaskListToDeleteAsync(TaskDeleteListDTO dto, string userId);
    TaskListResDTO<TaskByUserIdDTO> GetResLisDTO(PageListDTO dtoQuery, int totalPages, bool hasNextPage, IEnumerable<TaskByUserIdDTO> tasksList);

}