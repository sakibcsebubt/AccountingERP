using Microsoft.Extensions.Logging;

namespace MVC.ERPWEB.Models
{
    public class EntAccCodeBookModel
    {
        public string? EntId { get; set; }
        public string? Entsnam { get; set; }
        public string? Accid  { get; set; }
        public string? AcHead { get; set; }
    }
    public class AccCodeBookModel
    {
        public string? EntId { get; set; }
        public string? Entsnam { get; set; }
        public string? Accid { get; set; }
        public string? AcHead { get; set; }
        public string? aclevel { get; set; }
        public string? actype { get; set; }
        public string? actypdes { get; set; }
        public string? recnum { get; set; } 
    }
}
