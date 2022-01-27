using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_MoMo
{
    public class momoInfo
    {
        public static readonly string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
        public static readonly string partnerCode = "MOMOMOGY20210806";
        public static readonly string accessKey = "hzIxOx9GIhkUASQU";
        public static readonly string serectkey = "06531S3FHehCTDAo5Ghgg6rRZf8AnkgQ";
        public static readonly string orderInfo = "Shop điện thoại momo";
        public static readonly string returnUrl = "https://localhost:44387/confirm-orderPaymentOnline-momo";
        public static readonly string notifyurl = "https://localhost:44387/cancel-order";
    }
}