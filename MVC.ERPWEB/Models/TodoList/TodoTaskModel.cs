using Microsoft.VisualBasic;

namespace MVC.ERPWEB.Models.TodoList
{
    public class TodoTaskModel
    {

        public long Id { get; set; }
        public long UserId { get; set; }
        public int ParentTaskId { get; set; }
        public int PriorityId { get; set; }
        public int WorkingStatusId { get; set; }
        public long? AssigneUserId { get; set; }
        public string? UserName { get; set; }
        public string? Designation { get; set; }
        public string? ProjectName { get; set; }
        public string? TaskName { get; set; }
        public decimal? ExpectedTime { get; set; }
        public decimal? WorkingTime { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateTaskModel
    {

        public long Id { get; set; }
        public string? DeveloperName { get; set; }
        public long DeveloperId { get; set; }
        public int ParentTaskId { get; set; }
        public string? ParentTaskName { get; set; }
        public int PriorityId { get; set; }
        public int WorkingStatusId { get; set; }
        public string? TaskName { get; set; }
        public decimal? ExpectedTime { get; set; }
        public decimal? WorkingTime { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long ModifiedBY { get; set; }
    }

    public class DeveloperMdoel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? EmpId { get; set; }
        public string? Designation { get; set; }
    }


    public class TodoListVM
    {
        public List<TodoTaskModel> TaskList { get; set; }
        public List<DeveloperMdoel> DeveloperList { get; set; }

    }
}
