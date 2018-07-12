using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChulWoo.Models
{
    public class Contract
    {
        // 키값(계약정보) 키값(계약정보)
        public int ID { get; set; }

        // 키값(직원테이블) 키값(직원테이블)
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; } 

        // 계약시작 계약시작
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        // 계약끝 계약끝
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        // 타입 타입
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Type { get; set; }

        // 급여 급여
        [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
        public int? Salary { get; set; }
    }
}