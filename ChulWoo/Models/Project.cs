using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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

        // 프로젝트이름 프로젝트이름
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string NameVn { get; set; }
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string NameKr { get; set; }

        // 날짜 날짜
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public virtual ICollection<UploadFile> UploadFiles { get; set; }


        [DefaultValue(true)]
        public bool Translate { get; set; }

    }
}