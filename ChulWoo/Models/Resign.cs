using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChulWoo.Models
{
    public class Resign
    {
        [Key]
        [ForeignKey("Employee")]
        public int? EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ResignDate { get; set; }

        // 퇴사사유 퇴사사유(베트남)
        public string ResignNoteVn { get; set; }

        // 퇴사사유 퇴사사유(한글)
        public string ResignNoteKr { get; set; }
    }
}