using Microsoft.AspNetCore.Mvc;
using MVC.ERPWEB.ApiCommonClasses;
using System.Reflection.Emit;

namespace MVC.ERPWEB.Controllers
{
    public class CodeBookController : Controller
    {
        private List<Entity02Accounts.accinf1> acclist1 = new();
        private readonly IConfiguration configuration;
        private readonly string dbName;
        public CodeBookController( IConfiguration _configuration)
        {
            configuration = _configuration;
            dbName = configuration["DatabaseName"] ?? "LIVEERPDB";
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ChartOfAccounts()
        {
            //var _unit = new WebProcessAccess();
            //var pap1 = SetParamSysAreaInfCodeBook("0000", "%", "12345");
            var pap1 = new ApiAccessParms
            {
                EntID = "0000",
                ProcName = "dbo.SP_LE_REPORT_CODEBOOK_01",
                ProcID = "ACCODLIST01",
                parm01 = "%",
                parm02 = "12345"
            }; 

            string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName);
            if (JsonDs1a == null)
                return View();

            acclist1 = AppCustomFunctions.JsonStringToList<Entity02Accounts.accinf1>(JsonDs1a, "Table");

            return View(acclist1);
        }
    }
}
