using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChulWoo.Models
{
    public class MaterialBuyUnit
    {
        // 키값(자재구매단가) 키값(자재구매단가)
        public int ID { get; set; }

        // 키값(단가) 키값(단가)
        public int? MaterialUnitPriceID { get; set; }
        public virtual MaterialUnitPrice MaterialUnitPrice { get; set; }

        // 수량 수량
        public int? Quantity { get; set; }

        // 무게 무게
        public int? Weight { get; set; }

        // 날짜 날짜
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }
    }
}