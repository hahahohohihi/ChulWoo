using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class User
    {
        public int ID { get; set; }

        // 키값(퇴직정보) 키값(퇴직정보)
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }


        [Required(ErrorMessage = "input User ID")]
        public string UserID { get; set; }

        public string UserPassword { get; set; }

    }
}