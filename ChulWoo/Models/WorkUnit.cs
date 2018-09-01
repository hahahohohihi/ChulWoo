using ChulWoo.LocalResource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class WorkUnit
    {
        public int ID { get; set; }

        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }


        [Display(Name = "StartDate", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "EndDate", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }


        [Display(Name = "Complete", ResourceType = typeof(Resource))]
        public bool Complete { get; set; }

        [Display(Name = "WorkName", ResourceType = typeof(Resource))]
        public string WorkNameVn { get; set; }

        [Display(Name = "WorkName", ResourceType = typeof(Resource))]
        public string WorkNameKr { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteVn { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resource))]
        public string NoteKr { get; set; }


        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        public bool Translate { get; set; }
    }
}