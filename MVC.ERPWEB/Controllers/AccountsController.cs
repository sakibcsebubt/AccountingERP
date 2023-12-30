using Microsoft.AspNetCore.Mvc;
using MVC.ERPWEB.ApiCommonClasses;
using MVC.ERPWEB.Helper;
using MVC.ERPWEB.Models;
using System.Collections.ObjectModel;
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
            model.BranchList = await CommonHelper.GetBranchlist(EntCode, "2");  
            model.EntAccountCodeBook = ActcodeList;
            model.EntLocationList = await CommonHelper.GetEntLocationList(); 
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
        
        //[NonAction]
        //public async Task<List<LocationModel>> GetBranchlist(string EntCode, string Level)
        //{
        //    var pap1 = new ApiAccessParms
        //    {
        //        EntID = EntCode??"0000",
        //        ProcName = "dbo.SP_LE_REPORT_CODEBOOK_01",
        //        ProcID = "LOCATIONLIST01",
        //        parm01 = "%",  // Branchcode 
        //        parm02 = Level??"%"

        //    };
        //    string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName);
        //    if (JsonDs1a == null)
        //        return new List<LocationModel>(); 

        //    var BranchList = AppCustomFunctions.JsonStringToList<LocationModel>(JsonDs1a, "Table");
        //    return BranchList;
        //}

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



        public JsonResult AtxtActCode_SelectChanges(string AtxtActCode)
        {
            if (AtxtActCode.Length == 0)
                return Json("");

            if (AtxtActCode.Trim().Length == 0)
                return Json("");

            string actVal = AtxtActCode.Trim();

            bool level2 = false;
            var acCodeInf = ActcodeList.Find(x => x.Accid == actVal);
            if (acCodeInf != null)
                level2 = (acCodeInf.Aclevel.Trim() == "2");

            if (level2 == true)
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
            //  this.lblLevel2.Visibility = (level2 ? Visibility.Visible : Visibility.Hidden);
            //  this.chkSubHead_Click(null, null);
        }


        public JsonResult GetItemSirdesc()
        {
            ObservableCollection<AccCodeBookModel> Itemlist1 = null;

            Itemlist1 = new ObservableCollection<AccCodeBookModel>(
                ActcodeList.Where((x, match) => x.Accid.Substring(9, 3) != "000").ToList().OrderBy(m => m.AcHead)); // sirinfo will append here 

            return Json(Itemlist1);
        }


        //private void CalculateTotal(string cmbVouType)
        //{
        //    try
        //    {
        //        //  this.dgTrans.ItemsSource = null;
        //        foreach (var item1 in ListVouTable1)
        //        {
        //            if (item1.actcode != "000000000000")
        //            {
        //                item1.trnam = item1.dramt - item1.cramt;
        //                item1.trnrate = (item1.trnqty != 0 ? Math.Round(item1.trnam / item1.trnqty, 2) : 0.00m);
        //                item1.DrCrOrder = (item1.trnam > 0 ? "01" : "02");
        //            }
        //        }

        //        string vType1 = cmbVouType;

        //        decimal sumDr = ListVouTable1.FindAll(x => x.actcode != "000000000000").Sum(x => x.dramt);
        //        decimal sumCr = ListVouTable1.FindAll(x => x.actcode != "000000000000").Sum(x => x.cramt);

        //        if (vType1.Substring(0, 1) == "R")
        //        {
        //            foreach (var item1d in ListVouTable1)
        //            {
        //                if (item1d.actcode == "000000000000")
        //                {
        //                    item1d.dramt = (sumCr - sumDr);
        //                    break;
        //                }

        //            }
        //        }
        //        else if (vType1.Substring(0, 1) == "P" || vType1.Substring(1, 1) == "T") //if (vType1.Substring(1, 1) == "D" || vType1.Substring(1, 1) == "T")
        //        {
        //            foreach (var item1d in ListVouTable1)
        //            {
        //                if (item1d.actcode == "000000000000")
        //                {
        //                    item1d.cramt = (sumDr - sumCr);
        //                    break;
        //                }
        //            }
        //        }
        //        ListVouTable1 = ListVouTable1.FindAll(x => (x.dramt + x.cramt) != 0).ToList();
        //        ListVouTable1.Sort(delegate (vmEntryVoucher1.VouTable x, vmEntryVoucher1.VouTable y)
        //        {
        //            return (x.DrCrOrder + x.cactcode + x.actcode).CompareTo(y.DrCrOrder + y.cactcode + y.actcode);
        //        });

        //        int i = 1;
        //        string prevActcode1 = "XXXXXXXXXXXX";
        //        foreach (var item1 in ListVouTable1)
        //        {
        //            item1.trnsl = i;
        //            if (item1.actcode != "000000000000")
        //            {
        //                string actcodeDesc1 = (item1.actcode == prevActcode1 ? "" : item1.actcodeDesc);
        //                item1.trnDesc = actcodeDesc1 + (item1.sircodeDesc.Length > 0 ? (actcodeDesc1.Length > 0 ? "\n\t" : "\t") + item1.sircodeDesc + (item1.sircode2Desc.Length > 0 ? "\n\t\t" + item1.sircode2Desc : "") : "");
        //            }
        //            prevActcode1 = item1.actcode;
        //            ++i;
        //        }

        //        //    this.lblSumDram.Content = this.ListVouTable1.Sum(x => x.dramt).ToString("#,##0.00");
        //        //    this.lblSumCram.Content = this.ListVouTable1.Sum(x => x.cramt).ToString("#,##0.00");
        //        //    this.dgTrans.ItemsSource = this.ListVouTable1;
        //        //    this.gridCalc1.Visibility = Visibility.Collapsed;

        //        bool QtyFound = (ListVouTable1.FindAll(x => x.trnqty != 0).Count > 0);

        //        //   this.dgTransColQty.Visibility = (QtyFound ? Visibility.Visible : Visibility.Hidden);
        //        //     this.dgTransColUnit.Visibility = (QtyFound ? Visibility.Visible : Visibility.Hidden);
        //        //    this.dgTransColRate.Visibility = (QtyFound ? Visibility.Visible : Visibility.Hidden);
        //        //     this.SeprQty.Width = (QtyFound ? 65 : 0);
        //        //    this.SeprUnit.Width = (QtyFound ? 45 : 0);
        //        //     this.SeprRate.Width = (QtyFound ? 80 : 0);
        //        //      this.dgTransColLoc.Width = (QtyFound ? 120 : 250);
        //        //------ Draft information update option is enabled (Generally for local/high avaliability of database)
        //        //    if (this.chkAllowDraft.IsChecked == true)
        //        //   this.UpdateDraftVoucherInformation();

        //    }
        //    catch (Exception exp)
        //    {
        //        //     ASITHmsWpc00.HmsWpfProcAccess.ShowCatchErrorMessage("ACV-11", exp);
        //    }
        //}
    }
}
