using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ChulWoo.LocalResource;

namespace ChulWoo.Models
{
    public class Board
    {
        public int ID { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        [Display(Name = "BoardTitle", ResourceType = typeof(Resource))]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "Want 3 ~ 256 characters.")]
        public string TitleVn { get; set; }
        [Display(Name = "BoardTitle", ResourceType = typeof(Resource))]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "Want 3 ~ 256 characters.")]
        public string TitleKr { get; set; }

        [Display(Name = "BoardNote", ResourceType = typeof(Resource))]
        [StringLength(int.MaxValue)]
        public string NoteVn { get; set; }
        [Display(Name = "BoardNote", ResourceType = typeof(Resource))]
        [StringLength(int.MaxValue)]
        public string NoteKr { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public virtual ICollection<UploadFile> UploadFiles { get; set; }


        [Display(Name = "Translate", ResourceType = typeof(Resource))]
        [DefaultValue(true)]
        public bool Translate { get; set; }
    }
}