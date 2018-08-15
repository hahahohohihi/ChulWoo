using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ChulWoo.LocalResource;

namespace ChulWoo.Models
{
    public class MaterialName
    {
        public int ID { get; set; }

        // 구분 구분
        [Display(Name = "MaterialName", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string NameVn { get; set; }

        [Display(Name = "MaterialName", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string NameKr { get; set; }

        // 구분 구분
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Sort { get; set; }

    }
}