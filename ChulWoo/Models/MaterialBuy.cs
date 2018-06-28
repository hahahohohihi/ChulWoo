using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class MaterialBuy
    {
        // 키값(자재구매) 키값(자재구매)
        public int ID { get; set; }

        // 키값(회사) 키값(회사)
        public int? CompanyID { get; set; }
        public virtual Company Company { get; set; }

        // 키값(프로젝트) 키값(프로젝트)
        public int? ProjectID { get; set; }
        public virtual Project Project { get; set; }

        // 키값(단가) 키값(단가)
        public virtual ICollection<MaterialBuyUnit> MaterialBuyUnits { get; set; }
    }
}