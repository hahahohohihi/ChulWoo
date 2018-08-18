using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ChulWoo.LocalResource;

namespace ChulWoo.Models
{
    public class MaterialBuy
    {
        // 키값(자재구매) 키값(자재구매)
        public int ID { get; set; }

        public int? EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        // 키값(회사) 키값(회사)
        public int? CompanyID { get; set; }
        public virtual Company Company { get; set; }

        // 키값(프로젝트) 키값(프로젝트)
        public int? ProjectID { get; set; }
        public virtual Project Project { get; set; }

        // 키값(단가) 키값(단가)
        public virtual ICollection<MaterialBuyUnit> MaterialBuyUnits { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        // 날짜 날짜
        [Display(Name = "Date", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [Display(Name = "SubNote", ResourceType = typeof(Resource))]
        public string NoteVn { get; set; }
        [Display(Name = "SubNote", ResourceType = typeof(Resource))]
        public string NoteKr { get; set; }

        [DefaultValue(true)]
        public bool VAT { get; set; }


        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        [DefaultValue(true)]
        public bool Translate { get; set; }
    }
}