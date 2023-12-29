using MVC.ERPWEB.ApiCommonClasses;
using MVC.ERPWEB.Models;
using System.Data;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;
using static MVC.ERPWEB.ApiCommonClasses.StaticList;

namespace MVC.ERPWEB.Helper
{
    public class CommonHelper
    {
        private static List<ChartOfAccountModel> _chartOfAccountsList;
        private CommonHelper() { }

        public static async Task<List<ChartOfAccountModel>> GetChartOfAccountsList()
        {
            if (_chartOfAccountsList == null)
            {
                _chartOfAccountsList = await GetDataFromDatabase();
            }
            return _chartOfAccountsList;
        }

        // Method to simulate retrieving data from the database
        private static async Task<List<ChartOfAccountModel>> GetDataFromDatabase()
        {
            var pap1 = new ApiAccessParms
            {
                EntID = "0000",
                ProcName = "dbo.SP_LE_REPORT_CODEBOOK_01",
                ProcID = "ACCODLIST01",
                parm01 = "%",
                parm02 = "1234"
            };
            var dbName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionInfo")["PrimaryDBName"];
            string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName ?? "LIVEERPDB");
            if (JsonDs1a == null)
                return new List<ChartOfAccountModel>();

            var _chartOfAccountsList = AppCustomFunctions.JsonStringToList<ChartOfAccountModel>(JsonDs1a, "Table");
            return _chartOfAccountsList;
        }

        public async static Task<List<SirInfCodeBook>> GetAccSirCodeList()
        {  
            var pap1 = new ApiAccessParms
            {
                EntID = "2501",
                ProcName = "dbo.SP_LE_REPORT_CODEBOOK_01",
                ProcID = "ACCODLIST01",
                parm01 = "[0123458][0-9]%",
                parm02 = "12345"
            };
            //var pap1 = vmCfg1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "[0123458][0-9]%", "12345");
            var dbName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionInfo")["PrimaryDBName"];
            string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName ?? "LIVEERPDB");
            if (JsonDs1a == null) 
                return null;

            var pap2 = new ApiAccessParms
            {
                EntID = "2501",
                ProcName = "dbo.SP_LE_REPORT_CODEBOOK_01",
                ProcID = "ACCODLIST01",
                parm01 = "9[589]%",
                parm02 = "12345"
            };
            //var pap1 = vmCfg1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "[0123458][0-9]%", "12345"); 
            string JsonDs1b = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName ?? "LIVEERPDB");
            if (JsonDs1b == null)
                return null;  

            var list1 = AppCustomFunctions.JsonStringToList<SirInfCodeBook>(JsonDs1a, "Table");
            var list2 = AppCustomFunctions.JsonStringToList<SirInfCodeBook>(JsonDs1a, "Table"); 
            //var list3 = ds3.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();

            var AccSirCodeList = list1.Union(list2).ToList(); //list1.Union(list2).Union(list3).ToList();
            return AccSirCodeList;
        }


        public async static Task<List<AccCodeBookModel>> GetAccountCodeBookList()
        {
            var pap1 = new ApiAccessParms
            {
                EntID = "2501",
                ProcName = "dbo.SP_LE_REPORT_CODEBOOK_01",
                ProcID = "ACCODLIST01",
                parm01 = "%",
                parm02 = "12345"
            };
            //var pap1 = vmCfg1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "[0123458][0-9]%", "12345");
            var dbName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionInfo")["PrimaryDBName"];
            string JsonDs1a = await WebProcessAccess.GetGerpAppJsonData(pap1, dbName ?? "LIVEERPDB");
            if (JsonDs1a == null)
                return null; 

            var AccountCodeBookList = AppCustomFunctions.JsonStringToList<AccCodeBookModel>(JsonDs1a, "Table");  
            return AccountCodeBookList;
        } 
    }
}
