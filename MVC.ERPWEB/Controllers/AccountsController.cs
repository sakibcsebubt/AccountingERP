using Microsoft.AspNetCore.Mvc;
using MVC.ERPWEB.ApiCommonClasses;
using MVC.ERPWEB.Helper;
using MVC.ERPWEB.Models;
using System.Collections.ObjectModel;
using static MVC.ERPWEB.ApiCommonClasses.StaticList;
using static MVC.ERPWEB.Models.VoucherEntry.VoucherEntryVM;

namespace MVC.ERPWEB.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly string dbName;
        private List<ChartOfAccountModel> CactcodeList = new();
        private static List<AccCodeBookModel> ActcodeList = new();
        private static List<VoucherTable> ListVouTable = new ();

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
            var data = await VoucherWiseSourchCashHead(voucherno);

            if (data != null)
            {
                return Ok(data); // OK (200) status with the JSON data
            }
            else
            {
                return NotFound(); // Or handle the null case based on your logic
            }
        }

        public async Task<List<ChartOfAccountModel>> VoucherWiseSourchCashHead(string vounum)
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
                    switch (vounum.Substring(0, 1))
                    {
                        case "P": 
                            CactcodeList1a = this.CactcodeList.FindAll(x => x.AccId.Substring(0, 5) == "12198");
                            //   this.lblCactCodeTitle.Content = (vounum.Contains("PVC") ? "_Source" : "Depo_sit") + " Cash";
                            break;
                        case "R": 
                            CactcodeList1a = this.CactcodeList.FindAll(x => x.AccId.Substring(0, 5) == "12199" || x.AccId.Substring(0, 5) == "22199");
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

        public JsonResult AddVoucherToTable(string cmbDrCr, string txtAmt, string txtQty, string cmbVouType, string AtxtCactCode, string AtxtCactCodeValue, string AtxtSectCod, string AtxtSectCodValue, string AtxtActCode, string AtxtActCodeValue, string AutoCompleteSirCodeValue, string AutoCompleteSirCode, string lblSlNo, string AutoCompleteSirCode2Text, string remarks)
        {
            try
            {
                string drcr = cmbDrCr.Substring(0, 1).ToUpper();
                decimal trnamt1 = decimal.Parse("0" + txtAmt);
                decimal dramt1 = (drcr == "D" ? trnamt1 : 0.00m);
                decimal cramt1 = (drcr == "C" ? trnamt1 : 0.00m);
                decimal trnqty1 = decimal.Parse("0" + txtQty);
                decimal trnrate1 = (trnqty1 > 0 && trnamt1 > 0 ? (trnamt1 / trnqty1) : 0.00m);
                string vType1 = cmbVouType.ToString();
                var cactcode1 = (AtxtCactCode.Length == 0 ? "000000000000" : (AtxtCactCode.Length != 12 ? "000000000000" : AtxtCactCode)); // source cash
                cactcode1 = (vType1.Contains("JV") || vType1.Contains("OP") ? "000000000000" : cactcode1);

                var cactcodeDesc1 = (cactcode1 == "000000000000" ? "" : AtxtCactCodeValue);
                var sectcod1 = (AtxtSectCod.Length == 0 ? "000000000000" : (AtxtSectCod.Length != 12 ? "000000000000" : AtxtSectCod));
                var sectcodDesc1 = (sectcod1 == "000000000000" ? "" : AtxtSectCodValue);
                var actcode1 = (AtxtActCode.Length == 0 ? "000000000000" : (AtxtActCode.Length != 12 ? "000000000000" : AtxtActCode));
                var actcodeDesc1 = (actcode1 == "000000000000" ? "" : AtxtActCodeValue.Trim());
                var sircode1a = AutoCompleteSirCodeValue;
                var sircode1 = (sircode1a == null ? "000000000000" : (sircode1a.ToString().Trim().Length != 12 ? "000000000000" : sircode1a.ToString().Trim()));
                var sircodeDesc1 = (sircode1 == "000000000000" ? "" : AutoCompleteSirCode.Trim());
                var sirUnit1 = (sircode1 == "000000000000" ? "" : ActcodeList.Find(x => x.Accid == sircode1).Recnum.Trim());// this.lblUnit.Content.ToString();

                //string reptsl1 = lblSlNo.Trim();
                string reptsl1 = "1";

                var sircode2a = ""; // this.AutoCompleteSirCode2.SelectedValue;
                var sircode2 = (sircode2a == null ? "000000000000" : (sircode2a.ToString().Trim().Length != 12 ? "000000000000" : sircode2a.ToString().Trim()));
                var sircode2Desc1 = (sircode2 == "000000000000" ? "" : AutoCompleteSirCode2Text.Trim());

                var rmrk1 = remarks; // this.txtRmrk.Text.Trim();
                if (actcode1 == "000000000000")
                    return Json("null 1");

                //if (this.stkpControl.Visibility == Visibility.Visible)
                //{
                //    if (cactcode1 == "000000000000")
                //        return;
                //}

                string ac1 = actcode1.Substring(0, 4);
                bool CashBank = ((ac1 == "1901" || ac1 == "1902" || ac1 == "2902") ? true : false);

                //CashBank = (vType1.Contains("FTV") && this.ShowFundTransferLocation == true ? false : CashBank);
                //if (this.stkpLocation.Visibility == Visibility.Visible && CashBank == false)
                //{
                //    if (sectcod1 == "000000000000")
                //        return;
                //}
                //sectcod1 = (CashBank == true ? "000000000000" : sectcod1);
                //sectcodDesc1 = (CashBank == true ? "" : sectcodDesc1);

                bool QtyFound = false;
                foreach (var itemd in ListVouTable)
                {
                    QtyFound = (itemd.trnqty != 0 || QtyFound ? true : false);
                    if (itemd.cactcode == cactcode1 && itemd.sectcod == sectcod1 && itemd.actcode == actcode1 && itemd.sircode == sircode1 && itemd.reptsl == reptsl1)
                        return Json("something wents");
                }

                if (sircode1 != "000000000000") //  && this.stkpQty.Visibility == Visibility.Visible
                {
                    var tsirCod1 = ActcodeList.Find(x => x.Accid == sircode1);
                    //  this.lblUnit.Content = tsirCod1.sirunit.Trim();
                }

                //  this.gridDetails1.Visibility = Visibility.Visible;
                //  this.btnUpdate.Visibility = Visibility.Visible;
                //  this.btnUpdate.IsEnabled = true;


                if (!vType1.Contains("JV") && !vType1.Contains("OP"))
                {

                    foreach (var item in ListVouTable)
                    {
                        item.cactcode = cactcode1;
                        item.cactcodeDesc = cactcodeDesc1;
                        if (item.actcode == "000000000000" && item.sectcod == "000000000000")
                            item.trnDesc = cactcodeDesc1;
                    }

                    var Ccod1 = ListVouTable.FindAll(x => x.cactcode == cactcode1 && x.actcode == "000000000000" && x.sectcod == "000000000000");
                    if (Ccod1.Count == 0)
                    {
                        ListVouTable.Add(new ()
                        {
                            trnsl = ListVouTable.Count() + 1,
                            DrCrOrder = (vType1.Substring(0, 1) == "R" ? "01" : "02"), // (vType1.Substring(1, 1) == "C" ? "01" : "02"),
                            cactcode = cactcode1,
                            sectcod = "000000000000",
                            actcode = "000000000000",
                            sircode = "000000000000",
                            reptsl = "000",
                            sircode2 = "000000000000",
                            cactcodeDesc = cactcodeDesc1,
                            sectcodDesc = "",
                            actcodeDesc = "",
                            sircodeDesc = "",
                            sircode2Desc = "",
                            trnDesc = cactcodeDesc1,
                            trnqty = 0,
                            trnUnit = "",
                            trnrate = 0,
                            dramt = 0,
                            cramt = 0,
                            trnam = 0,
                            trnrmrk = ""
                        });
                    }
                }

                var reptsl1a = (ListVouTable.Count == 0 ? "000" : ListVouTable.Max(x => x.reptsl));
                reptsl1 = (int.Parse(reptsl1a) + 1).ToString("000");

                ListVouTable.Add(new ()
                {
                    trnsl = ListVouTable.Count() + 1,
                    DrCrOrder = ((dramt1 - cramt1) > 0 ? "01" : "02"),
                    cactcode = cactcode1,
                    sectcod = sectcod1,
                    actcode = actcode1,
                    sircode = sircode1,
                    reptsl = reptsl1,
                    sircode2 = sircode2,
                    cactcodeDesc = cactcodeDesc1,
                    sectcodDesc = sectcodDesc1,
                    actcodeDesc = actcodeDesc1,
                    sircodeDesc = sircodeDesc1,
                    sircode2Desc = sircode2Desc1,
                    trnDesc = actcodeDesc1 + (sircodeDesc1.Length > 0 ? "\n\t" + sircodeDesc1 + (sircode2Desc1.Length > 0 ? "\n\t\t" + sircode2Desc1 : "") : ""),
                    trnqty = trnqty1,
                    trnUnit = sirUnit1,
                    trnrate = trnrate1,
                    dramt = dramt1,
                    cramt = cramt1,
                    trnam = trnamt1,
                    trnrmrk = rmrk1
                });
                return Json(ListVouTable);
                //  this.lblSlNo.Content = "xxx";
                //  this.CleanupControls2();
                this.CalculateTotal(cmbVouType);
                //  this.txtActCode.Focus();
            }
            catch (Exception exp)
            {
                //  ASITHmsWpc00.HmsWpfProcAccess.ShowCatchErrorMessage("ACV-10", exp);
            }


            return Json(ListVouTable);
        }


        private void CalculateTotal(string cmbVouType)
        {
            try
            {
                //  this.dgTrans.ItemsSource = null;
                foreach (var item1 in ListVouTable)
                {
                    if (item1.actcode != "000000000000")
                    {
                        item1.trnam = item1.dramt - item1.cramt;
                        item1.trnrate = (item1.trnqty != 0 ? Math.Round(item1.trnam / item1.trnqty, 2) : 0.00m);
                        item1.DrCrOrder = (item1.trnam > 0 ? "01" : "02");
                    }
                }

                string vType1 = cmbVouType;

                decimal sumDr = ListVouTable.FindAll(x => x.actcode != "000000000000").Sum(x => x.dramt);
                decimal sumCr = ListVouTable.FindAll(x => x.actcode != "000000000000").Sum(x => x.cramt);

                if (vType1.Substring(0, 1) == "R")
                {
                    foreach (var item1d in ListVouTable)
                    {
                        if (item1d.actcode == "000000000000")
                        {
                            item1d.dramt = (sumCr - sumDr);
                            break;
                        }

                    }
                }
                else if (vType1.Substring(0, 1) == "P" || vType1.Substring(1, 1) == "T") //if (vType1.Substring(1, 1) == "D" || vType1.Substring(1, 1) == "T")
                {
                    foreach (var item1d in ListVouTable)
                    {
                        if (item1d.actcode == "000000000000")
                        {
                            item1d.cramt = (sumDr - sumCr);
                            break;
                        }
                    }
                }
                ListVouTable = ListVouTable.FindAll(x => (x.dramt + x.cramt) != 0).ToList();
                ListVouTable.Sort(delegate (VoucherTable x, VoucherTable y)
                {
                    return (x.DrCrOrder + x.cactcode + x.actcode).CompareTo(y.DrCrOrder + y.cactcode + y.actcode);
                });

                int i = 1;
                string prevActcode1 = "XXXXXXXXXXXX";
                foreach (var item1 in ListVouTable)
                {
                    item1.trnsl = i;
                    if (item1.actcode != "000000000000")
                    {
                        string actcodeDesc1 = (item1.actcode == prevActcode1 ? "" : item1.actcodeDesc);
                        item1.trnDesc = actcodeDesc1 + (item1.sircodeDesc.Length > 0 ? (actcodeDesc1.Length > 0 ? "\n\t" : "\t") + item1.sircodeDesc + (item1.sircode2Desc.Length > 0 ? "\n\t\t" + item1.sircode2Desc : "") : "");
                    }
                    prevActcode1 = item1.actcode;
                    ++i;
                }

                //    this.lblSumDram.Content = this.ListVouTable1.Sum(x => x.dramt).ToString("#,##0.00");
                //    this.lblSumCram.Content = this.ListVouTable1.Sum(x => x.cramt).ToString("#,##0.00");
                //    this.dgTrans.ItemsSource = this.ListVouTable1;
                //    this.gridCalc1.Visibility = Visibility.Collapsed;

                bool QtyFound = (ListVouTable.FindAll(x => x.trnqty != 0).Count > 0);

                //   this.dgTransColQty.Visibility = (QtyFound ? Visibility.Visible : Visibility.Hidden);
                //     this.dgTransColUnit.Visibility = (QtyFound ? Visibility.Visible : Visibility.Hidden);
                //    this.dgTransColRate.Visibility = (QtyFound ? Visibility.Visible : Visibility.Hidden);
                //     this.SeprQty.Width = (QtyFound ? 65 : 0);
                //    this.SeprUnit.Width = (QtyFound ? 45 : 0);
                //     this.SeprRate.Width = (QtyFound ? 80 : 0);
                //      this.dgTransColLoc.Width = (QtyFound ? 120 : 250);
                //------ Draft information update option is enabled (Generally for local/high avaliability of database)
                //    if (this.chkAllowDraft.IsChecked == true)
                //   this.UpdateDraftVoucherInformation();

            }
            catch (Exception exp)
            {
                //     ASITHmsWpc00.HmsWpfProcAccess.ShowCatchErrorMessage("ACV-11", exp);
            }
        }
    }
}
