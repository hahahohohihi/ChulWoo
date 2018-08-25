using ChulWoo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Viewmodel
{
    public class PaymentListData
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SearchDate { get; set; }
        public IEnumerable<Payment> Payments { get; set; }
    }
}