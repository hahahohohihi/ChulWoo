using ChulWoo.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public enum WorkType
    {
        [Display(Name = "DayWork", ResourceType = typeof(Resource))]
        DayWork,
        [Display(Name = "DayOvertime", ResourceType = typeof(Resource))]
        DayOvertime,
        [Display(Name = "DayAllNight", ResourceType = typeof(Resource))]
        DayAllNight,
        [Display(Name = "NightWork", ResourceType = typeof(Resource))]
        NightWork,
        [Display(Name = "NightOvertime", ResourceType = typeof(Resource))]
        NightOvertime,
    }
    public class FactoryWorkUnit
    {
        public int ID { get; set; }

        public int FactoryDailyWorkID { get; set; }
        public virtual FactoryDailyWork FactoryDailyWork { get; set; }

        // 키값(프로젝트) 키값(프로젝트)
        public int? ProjectID { get; set; }
        public virtual Project Project { get; set; }

        [Display(Name = "WorkType", ResourceType = typeof(Resource))]
        public WorkType Type { get; set; }

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