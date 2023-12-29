using Microsoft.Extensions.Logging;

namespace MVC.ERPWEB.Models
{
    public class LocationModel
    {
        public long Id { get; set; }
        public string Sectid { get; set; }
        public string Sectsdesc { get; set; }
        public string Sectdesc { get; set; }
        public string Recnum { get; set; }
    }
    
    public class ChartOfAccountModel
    {
        public long EntId { get; set; }
        public string AccId { get; set; }
        public string AcHead { get; set; }
        public string AcLevel { get; set; }
        public string AcType { get; set; }
        public string Actypdes { get; set; }
        public string Recnum { get; set; }
    } 
    
    public class AcInfCodeBook
    {
        public string comcod { get; set; }
        public string actcode { get; set; }
        public string actdesc { get; set; }
        public string actelev { get; set; }
        public string acttype { get; set; }
        public string acttdesc { get; set; }
        public long rowid { get; set; }
        public DateTime rowtime { get; set; }
        public string actcode1 { get; set; }
        public string actdesc1 { get; set; }
    } 
}
