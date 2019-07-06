
using SharpTask.Core.Models.Task;

namespace SharpTask.Core.Repository.TaskExecution
{
    public interface ITaskExecutionRepository
    {
        TaskClassState GetTaskClasses(TaskClassState taskClassState);
        TaskClassState StoreTaskClasses(TaskClassState taskClassState);
    }
}
