using ChulWoo.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public enum Security
    {
        Public, Personnel, Translation, Power, Admin
    }

    public class User
    {
        public int ID { get; set; }

        // 키값(퇴직정보) 키값(퇴직정보)
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }


        [Display(Name = "UserID", ResourceType = typeof(Resource))]
        [Required(ErrorMessage = "input User ID")]
        public string UserID { get; set; }

        [Display(Name = "UserPassword", ResourceType = typeof(Resource))]
        public string UserPassword { get; set; }

        public Security? Security { get; set; }
    }
}