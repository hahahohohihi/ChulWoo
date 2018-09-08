using ChulWoo.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class FactoryWorkUnit
    {
        public int ID { get; set; }

        public int FactoryDailyWorkID { get; set; }
        public virtual FactoryDailyWork FactoryDailyWork { get; set; }

        [Display(Name = "EquipCount", ResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public double EquipCount { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteVn { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteKr { get; set; }

        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        public bool Translate { get; set; }
    }
}