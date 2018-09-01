using ChulWoo.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class EquipmentUnit
    {
        public int ID { get; set; }

        public int DailyWorkID { get; set; }
        public virtual DailyWork DailyWork { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string NameVn { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string NameKr { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteVn { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteKr { get; set; }

        [Display(Name = "EquipCount", ResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double EquipCount { get; set; }

        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        public bool Translate { get; set; }
    }
}