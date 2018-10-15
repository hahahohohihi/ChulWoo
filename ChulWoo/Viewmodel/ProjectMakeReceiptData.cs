using ChulWoo.LocalResource;
using ChulWoo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Viewmodel
{
    public class ProjectMakeReceiptData
    {
        public Project Porject { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [Display(Name = "MaterialName", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string NameVn { get; set; }

        // 가격 가격
        [Display(Name = "UnitPrice", ResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resource))]
        public Currency Currency { get; set; }

        public double VATPer { get; set; }

        [Display(Name = "SubNote", ResourceType = typeof(Resource))]
        [StringLength(int.MaxValue)]
        public string NoteVn { get; set; }

    }
}