using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChulWoo.Models
{
    public enum PersonnelType
    {
        AnnualLeave, SickLeave, MaternityLeave, CompassionateLeave, MarriageLeave, NopayLeave, HospitalizationLeave, OtherLeave
    }

    public class Personnel
    {
        // 키값(인사) 키값(인사)
        public int ID { get; set; }

        // 키값(직원테이블) 키값(직원테이블)
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        // 시작날짜 시작날짜
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SendDate { get; set; }

        [StringLength(256)]
        public string Reason { get; set; }

        // 시작날짜 시작날짜
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] 
        public DateTime? StartDate { get; set; }

        // 끝날짜 끝날짜
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        // 타입 타입 
        public PersonnelType? Type { get; set; }
    }
}