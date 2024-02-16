namespace MVC.ERPWEB.ApiCommonClasses
{
    public static class AppBasicInfo
    {
        public static string AppTitle1 = "Group ERP Automation System";
        public static string AppTitle2 = "Group ERP Automation System";
        public static string ModuleOptions = "AIS,SCM,HCM,MIS";

        public static string AppVersion = "230927.1";
        public static string VersionType = "0"; // Development & Testing Version (0 from Published Version)
        public static string CopyType = (VersionType == "1" ? "(DEVELOPMENT COPY)" : (VersionType == "2" ? "(TRIAL COPY)" : "LIVECOPY"));// "(TRIAL COPY)"   "DEV COPY";

        public static string AppTerminalSlNo = "";
        public static string AppTerminalName = "";
        public static string AppTerminalsDes = "";

        public static string ConnStr1a = "MSSQLConnStr";
        public static string usrid1a = "db_a9e723_liveerpdp1_admin";
        public static string usrpass1a = "aspnetbd@321";
        public static string TimeOut1a = "300";
        public static string MaxPool1a = "200";
        public static string ConnType1a = "Web";

        public static List<string> AppEntIDs = new List<string> { "2501", "2502" };
        public static List<AppCustomClasses.DatabaseErrorInfo> DatabaseErrorInfoList;
        public static string AppUpdatePath = "http://aspnetbd-001-site3.ftempurl.com/GroupErpWpfGenApp.application";
        public static HttpClient GerpApiClient = new HttpClient();
        public static AppCustomClasses.JwtTokenInfo ApiToken;
        public static List<AppCustomClasses.TerminalComponent> TerminalComponentList1 = new List<AppCustomClasses.TerminalComponent>();

        public static void ConnectApi()
        {
            AppBasicInfo.GerpApiClient.BaseAddress = new Uri("http://localhost:5063/api/");
            //AppBasicInfo.GerpApiClient.BaseAddress = new Uri("http://localhost:5063/api/");
            AppBasicInfo.GerpApiClient.DefaultRequestHeaders.Accept.Clear();
            AppBasicInfo.GerpApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}
