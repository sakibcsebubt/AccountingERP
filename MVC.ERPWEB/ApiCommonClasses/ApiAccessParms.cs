namespace MVC.ERPWEB.ApiCommonClasses
{
    public class ApiAccessParms
    {
        public string EntID { get; set; }
        public string ProcName { get; set; }
        public string ProcID { get; set; }
        public string DbName { get; set; } = "";
        public string UserID { get; set; } = "";
        public string UserPwd { get; set; } = "";
        public string parmJson1 { get; set; }
        public string parmJson2 { get; set; }
        public byte[] parmBin01 { get; set; }
        public string parm01 { get; set; }
        public string parm02 { get; set; }
        public string parm03 { get; set; }
        public string parm04 { get; set; }
        public string parm05 { get; set; }
        public string parm06 { get; set; }
        public string parm07 { get; set; }
        public string parm08 { get; set; }
        public string parm09 { get; set; }
        public string parm10 { get; set; }

        public ApiAccessParms()
        {

        }
        public ApiAccessParms(string _EntID = "XXXX", string _ProcName = "XXXXXX", string _ProcID = "XXXXXX", string _parmJson1 = "",
            string _parmJson2 = "", byte[] _parmBin01 = null, string _parm01 = "", string _parm02 = "", string _parm03 = "", string _parm04 = "",
            string _parm05 = "", string _parm06 = "", string _parm07 = "", string _parm08 = "", string _parm09 = "", string _parm10 = "")
        {
            this.EntID = _EntID;
            this.ProcName = _ProcName;
            this.ProcID = _ProcID;
            this.parmJson1 = _parmJson1;
            this.parmJson2 = _parmJson2;
            this.parmBin01 = _parmBin01;
            this.parm01 = _parm01;
            this.parm02 = _parm02;
            this.parm03 = _parm03;
            this.parm04 = _parm04;
            this.parm05 = _parm05;
            this.parm06 = _parm06;
            this.parm07 = _parm07;
            this.parm08 = _parm08;
            this.parm09 = _parm09;
            this.parm10 = _parm10;
        }
    }
}
