using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ChulWoo.LocalResource;

namespace ChulWoo.Models
{
    public enum ContractType
    {
        [Display(Name = "Shorttime", ResourceType = typeof(Resource))]
        Shorttime,
        [Display(Name = "Apprentice", ResourceType = typeof(Resource))]
        Apprentice,
        [Display(Name = "Regular", ResourceType = typeof(Resource))]
        Regular, 
    }

    public class Contract
    {
        // 키값(계약정보) 키값(계약정보)
        public int ID { get; set; }

        // 키값(직원테이블) 키값(직원테이블)
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        // 계약시작 계약시작
        [Display(Name = "StartDate", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        // 계약끝 계약끝
        [Display(Name = "EndDate", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        // 타입 타입
        [Display(Name = "ContractType", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public ContractType Type { get; set; }

        // 급여 급여
        [Display(Name = "Salary", ResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
        public int? Salary { get; set; }
    }
}