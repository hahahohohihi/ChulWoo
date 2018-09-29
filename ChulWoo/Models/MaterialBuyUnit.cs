using ChulWoo.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class MaterialBuyUnit
    {
        // 키값(자재구매단가) 키값(자재구매단가)
        public int ID { get; set; }

        // 키값(단가) 키값(단가)
        public int? MaterialBuyID { get; set; }
        public virtual MaterialBuy MaterialBuy { get; set; }

        public int? MaterialUnitPriceID { get; set; }
        public virtual MaterialUnitPrice MaterialUnitPrice { get; set; }

        // 수량 수량
        [Display(Name = "Quantity", ResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public double Quantity { get; set; }
    }
}