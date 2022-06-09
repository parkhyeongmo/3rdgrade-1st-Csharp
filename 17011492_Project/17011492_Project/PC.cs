using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17011492
{
    class PC // PC Class
    {
        public int Id { get; set; }
        public bool power { get; set; } // 전원
        public bool inUse { get; set; } // 사용 여부
        public string Payment { get; set; } // 결제 수단

        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ChargeTime { get; set; } // 사용 등록된 시간, 초 단위
    }
}
