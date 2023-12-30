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
        public string? Aclevel { get; set; }
        public string? Actype { get; set; }
        public string? Actypdes { get; set; }
        public string? Recnum { get; set; } 
    }
}
