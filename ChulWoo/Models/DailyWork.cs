using ChulWoo.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class DailyWork
    {
        public int ID { get; set; }

        // 키값(프로젝트) 키값(프로젝트)
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public int ProparingPersonID { get; set; }
        public virtual Employee ProparingPerson { get; set; }

        public virtual ICollection<EquipmentUnit> EquipmentUnits { get; set; }
        public virtual ICollection<UploadFile> UploadFiles { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [Display(Name = "PrrocessPerPlan", ResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float PrrocessPerPlan { get; set; }

        [Display(Name = "PrrocessPerResult", ResourceType = typeof(Resource))]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public float PrrocessPerResult { get; set; }

        [Display(Name = "Weather", ResourceType = typeof(Resource))]
        public string WeatherVn { get; set; }

        [Display(Name = "Weather", ResourceType = typeof(Resource))]
        public string WeatherKr { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteVn { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteKr { get; set; }

        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        public bool Translate { get; set; }

    }
}