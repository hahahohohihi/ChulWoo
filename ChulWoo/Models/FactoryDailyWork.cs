using ChulWoo.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class FactoryDailyWork
    {
        public int ID { get; set; }

        public int ProparingPersonID { get; set; }
        public virtual Employee ProparingPerson { get; set; }

        public virtual ICollection<FactoryWorkUnit> FactoryWorkUnits { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteVn { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteKr { get; set; }

        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        public bool Translate { get; set; }
    }
}