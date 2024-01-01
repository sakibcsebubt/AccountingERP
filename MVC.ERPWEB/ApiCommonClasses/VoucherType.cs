namespace MVC.ERPWEB.ApiCommonClasses
{
    public class StaticList
    {
        public class VoucherType
        {
            public long Sl { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }
        public static List<VoucherType> VoucherList()
        {
            List<VoucherType> VoucherList = new()
            {
                new VoucherType() { Sl = 1, Code = "P", Name = "Cash Payment Voucher" },
                new VoucherType() { Sl = 2, Code = "P", Name = "Bank Payment Voucher" },
                new VoucherType() { Sl = 3, Code = "FB", Name = "Fund Transfer Voucher" },
                new VoucherType() { Sl = 4, Code = "R", Name = "Cash Receipt Voucher" },
                new VoucherType() { Sl = 5, Code = "R", Name = "Bank Receipt Voucher" },
                new VoucherType() { Sl = 6, Code = "JB", Name = "Adjustment Journal Voucher" },
                new VoucherType() { Sl = 7, Code = "OB", Name = "Accounts Opening Voucher" },
            };
            return VoucherList;
        }
    }
}
