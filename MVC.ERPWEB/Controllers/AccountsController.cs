using Microsoft.AspNetCore.Mvc;
using MVC.ERPWEB.ApiCommonClasses;
using MVC.ERPWEB.Helper;
using MVC.ERPWEB.Models;
using static MVC.ERPWEB.Models.VoucherEntry.VoucherEntryVM;

namespace MVC.ERPWEB.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly string dbName;
        private List<ChartOfAccountModel> CactcodeList = new();
        private static List<AccCodeBookModel> ActcodeList = new();
        public AccountsController(IConfiguration _configuration)
        {
            configuration = _configuration;
            dbName = configuration["DatabaseName"] ?? "LIVEERPDB";
            LoadAccountModule();
        }

        public async void LoadAccountModule() 
        {
            if (ActcodeList.Count == 0)
                ActcodeList = await CommonHelper.GetAccountCodeBookList();
        }



        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> VoucherEntry() 
        {
            var EntCode = configuration.GetSection("CompanyInfo")["EnterpriseCode"];
            var testCode = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("CompanyInfo")["EnterpriseCode"];
            VoucherEntryViewModel model = new();
            model.VoucherList = ApiCommonClasses.StaticList.VoucherList();
            model.BranchList = await GetBranchlist(EntCode, "2");  
            model.EntAccountCodeBook = ActcodeList;  
            model.ChartOfAccountList = await CommonHelper.GetChartOfAccountsList(); 
            return View(model);
        }

        [NonAction]
        public async Task<List<LocationModel>> GetSourceCashList(string PaymentType, string Branch)
        {
            var EntCode = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("CompanyInfo")["EnterpriseCode"];
            var CashCode = "22199";
            var Level = "345";
            var pap1 = new ApiAccessParms
            {
                EntID = EntCode??"0000",
                ProcName = "dbo.SP_LE_REPORT_CODEBOOK_01",
                ProcID = "LOCATIONLIST01",
                parm01 = CashCode,   
                parm02 = Level??"%"

            };
            string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName);
            if (JsonDs1a == null)
                return new List<LocationModel>(); 

            var BranchList = AppCustomFunctions.JsonStringToList<LocationModel>(JsonDs1a, "Table");
            return BranchList;
        }   
        
        [NonAction]
        public async Task<List<LocationModel>> GetBranchlist(string EntCode, string Level)
        {
            var pap1 = new ApiAccessParms
            {
                EntID = EntCode??"0000",
                ProcName = "dbo.SP_LE_REPORT_CODEBOOK_01",
                ProcID = "LOCATIONLIST01",
                parm01 = "%",  // Branchcode 
                parm02 = Level??"%"

            };
            string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName);
            if (JsonDs1a == null)
                return new List<LocationModel>(); 

            var BranchList = AppCustomFunctions.JsonStringToList<LocationModel>(JsonDs1a, "Table");
            return BranchList;
        }

        public async Task<IActionResult> GetCashHead(string voucherno)
        {
            var data = await BindControlCode(voucherno);

            if (data != null)
            {
                return Ok(data); // OK (200) status with the JSON data
            }
            else
            {
                return NotFound(); // Or handle the null case based on your logic
            }
        }

        public async Task<List<ChartOfAccountModel>> BindControlCode(string vounum)
        {
            try
            {
                if(CactcodeList.Count == 0)
                {
                    CactcodeList = await CommonHelper.GetChartOfAccountsList(); 
                } 
                if (!vounum.Contains("JV") && !vounum.Contains("OP"))
                {
                    List<ChartOfAccountModel>? CactcodeList1a = new();
                    switch (vounum.Substring(0, 3))
                    {
                        case "RVC":
                        case "PVC":
                            CactcodeList1a = this.CactcodeList.FindAll(x => x.AccId.Substring(0, 4) == "1010");
                            //   this.lblCactCodeTitle.Content = (vounum.Contains("PVC") ? "_Source" : "Depo_sit") + " Cash";
                            break;
                        case "RVB":
                        case "PVB":
                            CactcodeList1a = this.CactcodeList.FindAll(x => x.AccId.Substring(0, 4) == "1902" || x.AccId.Substring(0, 4) == "2902");
                            //  this.lblCactCodeTitle.Content = (vounum.Contains("PVB") ? "_Source" : "Depo_sit") + " Bank";
                            break;
                        case "FTV":
                            CactcodeList1a = this.CactcodeList.FindAll(x => x.AccId.Substring(0, 4) == "1901" || x.AccId.Substring(0, 4) == "1902" || x.AccId.Substring(0, 4) == "2902");
                            //    this.lblCactCodeTitle.Content = "From Cash/Bank";
                            //    this.lblActCodeTitle.Content = "To Cas_h/Bank";
                            break;
                    }

                    //foreach (var item1 in CactcodeList1a)
                    //{
                    // //   this.AtxtCactCode.AddSuggstionItem(item1.actdesc1.Trim(), item1.actcode);
                    // //   var mitm1 = new MenuItem() { Header = item1.actdesc1.Trim(), Tag = item1.actcode.Trim() };
                    // //   mitm1.Click += conMenuCactCode_MouseClick;
                    // //   this.conMenuCactCode.Items.Add(mitm1);
                    //}
                    return CactcodeList1a;
                }
            }
            catch (Exception exp)
            {
                // ASITHmsWpc00.HmsWpfProcAccess.ShowCatchErrorMessage("ACV-05", exp);
                return null;
            }
            return null;
        }
    }
}
