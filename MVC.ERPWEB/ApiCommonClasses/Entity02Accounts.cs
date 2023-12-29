namespace MVC.ERPWEB.ApiCommonClasses
{
    public class Entity02Accounts
    {
        public class accinf1
        {
            //select entid, accid, achead, aclevel, actype, actypdes, recnum from @tblacc01 order by accid;
            public string entid { get; set; }
            public string accid { get; set; }
            public string achead { get; set; }
            public string aclevel { get; set; }
            public string actype { get; set; }
            public string actypdes { get; set; }
            public Int64 recnum { get; set; }
        }
    }
}
