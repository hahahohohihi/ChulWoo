using ChulWoo.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public enum PaymentType
    {
        [Display(Name = "Cash", ResourceType = typeof(Resource))]
        Cash,
        [Display(Name = "Transfer", ResourceType = typeof(Resource))]
        Bank,
    }

    public class Payment
    {
        public int ID { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        // 키값(단가) 키값(단가)
        public int? MaterialBuyID { get; set; }
        public virtual MaterialBuy MaterialBuy { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
        public double Amount { get; set; }

        [Display(Name = "PaymentType", ResourceType = typeof(Resource))]
        public PaymentType? Type { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        [StringLength(int.MaxValue)]
        public string NoteVn { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resource))]
        [StringLength(int.MaxValue)]
        public string NoteKr { get; set; }

        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        public bool Translate { get; set; }

    }
}