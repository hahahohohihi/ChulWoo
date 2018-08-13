using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChulWoo.Models
{
    public enum Unit
    {
        Quantity, Weight, Set, Time
    }

    public class MaterialUnitPrice
    {
        public int ID { get; set; }

        // 키값(자재이름) 키값(자재이름)
        public int? MaterialNameID { get; set; }
        public virtual MaterialName MaterialName { get; set; }

        // 키값(회사) 키값(회사)
        public int? CompanyID { get; set; }
        public virtual Company Company { get; set; }

        // 날짜 날짜
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        // 단위 단위
        public Unit Unit { get; set; }

        // 가격 가격
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public float Price { get; set; }

    }
}