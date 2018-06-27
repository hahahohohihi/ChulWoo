using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChulWoo.Models
{
    public class MaterialName
    {
        public int ID { get; set; }

        // 구분 구분
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Sort { get; set; }
    }
}