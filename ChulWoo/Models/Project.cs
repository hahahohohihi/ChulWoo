using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ChulWoo.LocalResource;

namespace ChulWoo.Models
{
    public class Project
    {
        // 키값(프로젝트) 키값(프로젝트)
        public int ID { get; set; }

        // 키값(회사) 키값(회사)
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<MaterialBuy> MaterialBuys { get; set; }
        public virtual ICollection<Payment> Deposits { get; set; }

        // 프로젝트이름 프로젝트이름
        [Display(Name = "ProjectName", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string NameVn { get; set; }
        [Display(Name = "ProjectName", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string NameKr { get; set; }

        // 날짜 날짜
        [Display(Name = "Date", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public virtual ICollection<UploadFile> UploadFiles { get; set; }


        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        [DefaultValue(true)]
        public bool Translate { get; set; }

    }
}