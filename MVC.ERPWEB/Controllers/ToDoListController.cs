using Microsoft.AspNetCore.Mvc;
using MVC.ERPWEB.ApiCommonClasses;
using MVC.ERPWEB.Models.TodoList;
using Newtonsoft.Json;

namespace MVC.ERPWEB.Controllers
{
    public class ToDoListController : Controller
    {
        //private List<TodoTaskModel> GlobalTaskList = new ();
        private readonly IConfiguration configuration;
        private readonly string dbName;
        public ToDoListController(IConfiguration _configuration)
        {
            configuration = _configuration;
            dbName = configuration["DatabaseName"]??"LIVEERPDB";
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ToDoList()
        {

            TodoListVM model = new();
            model.DeveloperList = GetDeveloperList();

            var UserId = "1";
            try
            {
                var pap1 = new ApiAccessParms
                { 
                    ProcName = "GetUserWiseToDoList",
                    ProcID = "GetTodoTaskListbyUserId",
                    parm01 = UserId.ToString()
                };
                string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName);
                if (JsonDs1a == null)
                    return View(model);

                 model.TaskList = AppCustomFunctions.JsonStringToList<TodoTaskModel>(JsonDs1a, "Table");

                return View(model);
            }
            catch(Exception ex) 
            {
                throw ex;
            } 
        }

        [HttpGet]
        public async Task<JsonResult> GetTaskById(long Id)
        { 
            try
            {
                if(Id == 0)
                    return Json("CreateNewTask");

                var pap1 = new ApiAccessParms
                { 
                    ProcName = "dbo.GetUserWiseToDoList",
                    ProcID = "TASK_OBJ_ID",
                    parm01 = Id.ToString()
                };

                string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName);
                if (JsonDs1a == null)
                    return Json("No Data Found");

                var TaskList = AppCustomFunctions.JsonStringToList<TodoTaskModel>(JsonDs1a, "Table");

                return Json(TaskList);
            }
            catch(Exception ex) 
            {
                throw ex;
            } 
        } 
        
        [HttpPost]
        public async Task<JsonResult> UpdateTask(UpdateTaskModel model)
        { 
            try
            {
                model.ModifiedDate = DateTime.Now; 
                var TaskData = JsonConvert.SerializeObject(model);
                var pap1 = new ApiAccessParms
                {
                    ProcName = "dbo.GetUserWiseToDoList",
                    ProcID = "TASK_SAVE_INF",
                    parmJson1 = TaskData,
                    parm01 = model.Id.ToString()
                };

                string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName);
                if (JsonDs1a == null)
                    return Json("No Data Found");
                dynamic obj = JsonConvert.DeserializeObject(JsonDs1a);
                string successValue = obj.Table[0].Column1; 

                return Json(successValue);
            }
            catch(Exception ex) 
            {
                throw ex;
            } 
        }

        [NonAction]
        public List<DeveloperMdoel> GetDeveloperList()
        {
            List<DeveloperMdoel> DeveloperList= new();
            DeveloperList.Add(new DeveloperMdoel { Id = 1, Name = "Sakib Hasan", Designation = "Software Engineer", EmpId = "TN-99075" });
            DeveloperList.Add(new DeveloperMdoel { Id = 2, Name = "Sharif Hossain", Designation = "Software Engineer", EmpId = "TN-99078" });
            DeveloperList.Add(new DeveloperMdoel { Id = 3, Name = "Ismail Hossain", Designation = "Software Engineer", EmpId = "TN-99079" });
            return DeveloperList;
        } 
    }
}
