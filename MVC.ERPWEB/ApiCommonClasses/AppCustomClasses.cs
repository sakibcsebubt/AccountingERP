namespace MVC.ERPWEB.ApiCommonClasses
{
    public class AppCustomClasses
    {
        public class JwtTokenInfo
        {
            public bool istokenstr { get; set; } = false;
            public string tokenstr { get; set; } = "Nothing";
            public DateTime createtime { get; set; } = DateTime.UtcNow;
            public DateTime expirytime { get; set; } = DateTime.UtcNow;
            public string tokenmsg { get; set; } = "Failed to create token";

        }
        public class JwtTokenUser
        {
            public string UserId { get; set; } = "NOUSER";
            public string Password { get; set; } = "#$^&%##%&**^%$*&";
            public int Expirhour { get; set; } = 2;
        }
        [Serializable]
        public class DatabaseErrorInfo
        {
            public Int32 errornumber { get; set; }
            public Int32 errorseverity { get; set; }
            public Int32 errorstate { get; set; }
            public string errorprocedure { get; set; }
            public string process_id { get; set; }
            public Int32 errorline { get; set; }
            public string errormessage { get; set; }

            // select error_number() as errornumber, error_severity() as errorseverity, error_state() as errorstate,
            // error_procedure() as errorprocedure, @Desc01 as process_id, error_line() as errorline, error_message() as errormessage;
        }
        [Serializable]
        public class TerminalComponent
        {
            public string tsl { get; set; }         // Terminal Serial No.
            public string sln { get; set; }         // Component Serial No.
            public string grp { get; set; }         // Group Name
            public string cnm { get; set; }         // Component Name
            public string cds { get; set; }         // Component Description
        }


    }
}
