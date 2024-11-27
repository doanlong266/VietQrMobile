using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietQrMobile.Models
{
    public class VietQRResponse
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public VietQRData Data { get; set; }
    }

    public class VietQRData
    {
        public string QrCode { get; set; }
        public string QrDataURL { get; set; }
    }

}
