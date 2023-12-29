using static MVC.ERPWEB.ApiCommonClasses.StaticList;

namespace MVC.ERPWEB.Models.VoucherEntry
{
    public class VoucherEntryVM
    {
        public class VoucherEntryViewModel
        {
            public List<VoucherType> VoucherList { get; set;}
            public List<LocationModel> BranchList { get; set;}
            public List<ChartOfAccountModel> ChartOfAccountList { get; set;}
            public List<AccCodeBookModel> EntAccountCodeBook { get; set;}
            public List<LocationModel> EntLocationList { get; set;}
        }
    }
}
