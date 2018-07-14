using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ChulWoo.LocalResource;

namespace ChulWoo.Models
{
    public enum PersonnelType
    {
        [Display(Description = "AnnualLeave", ResourceType = typeof(Resource))]
        AnnualLeave,
        [Display(Description = "SickLeave", ResourceType = typeof(Resource))]
        SickLeave,
        [Display(Description = "MaternityLeave", ResourceType = typeof(Resource))]
        MaternityLeave,
        [Display(Description = "CompassionateLeave", ResourceType = typeof(Resource))]
        CompassionateLeave,
        [Display(Description = "MarriageLeave", ResourceType = typeof(Resource))]
        MarriageLeave,
        [Display(Description = "NopayLeave", ResourceType = typeof(Resource))]
        NopayLeave,
        [Display(Description = "HospitalizationLeave", ResourceType = typeof(Resource))]
        HospitalizationLeave,
        [Display(Description = "OtherLeave", ResourceType = typeof(Resource))]
        OtherLeave
    }

    public class Personnel
    {
        // 키값(인사) 키값(인사)
        public int ID { get; set; }

        // 키값(직원테이블) 키값(직원테이블)
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        // 게시날짜
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SendDate { get; set; }

        [StringLength(256)]
        public string Reason { get; set; }

        // 시작날짜 시작날짜
        [Display(Name = "StartDate", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] 
        public DateTime? StartDate { get; set; }

        // 끝날짜 끝날짜
        [Display(Name = "EndDate", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        // 타입 타입 
        [Display(Name = "Type", ResourceType = typeof(Resource))]
        public PersonnelType? Type { get; set; }
    }
}