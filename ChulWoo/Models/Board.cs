using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class Board
    {
        public int ID { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        [StringLength(256, MinimumLength = 3, ErrorMessage = "Want 3 ~ 256 characters.")]
        public string TitleVn { get; set; }
        [StringLength(256, MinimumLength = 3, ErrorMessage = "Want 3 ~ 256 characters.")]
        public string TitleKr { get; set; }

        [StringLength(int.MaxValue)]
        public string NoteVn { get; set; }
        [StringLength(int.MaxValue)]
        public string NoteKr { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public virtual ICollection<UploadFile> UploadFiles { get; set; }

    }
}