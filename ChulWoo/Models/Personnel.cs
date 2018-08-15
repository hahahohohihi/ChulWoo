using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ChulWoo.LocalResource;
using System.ComponentModel;
using ChulWoo.Helper;

namespace ChulWoo.Models
{
//    [LocalizationEnum(typeof(Resource))]
    public enum PersonnelType
    {
        [Display(Name = "AnnualLeave", ResourceType = typeof(Resource))]
        AnnualLeave,
        [Display(Name = "SickLeave", ResourceType = typeof(Resource))]
        SickLeave,
        [Display(Name = "MaternityLeave", ResourceType = typeof(Resource))]
        MaternityLeave,
        [Display(Name = "CompassionateLeave", ResourceType = typeof(Resource))]
        CompassionateLeave,
        [Display(Name = "MarriageLeave", ResourceType = typeof(Resource))]
        MarriageLeave,
        [Display(Name = "NopayLeave", ResourceType = typeof(Resource))]
        NopayLeave,
        [Display(Name = "HospitalizationLeave", ResourceType = typeof(Resource))]
        HospitalizationLeave,
        [Display(Name = "OtherLeave", ResourceType = typeof(Resource))]
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
        [Display(Name = "SendDate", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SendDate { get; set; }

        [Display(Name = "Reason", ResourceType = typeof(Resource))]
        [StringLength(256)]
        public string ReasonVn { get; set; }

        [Display(Name = "Reason", ResourceType = typeof(Resource))]
        [StringLength(256)]
        public string ReasonKr { get; set; }

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

        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Name { get; set; }


        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        [DefaultValue(true)]
        public bool Translate { get; set; }

    }
}