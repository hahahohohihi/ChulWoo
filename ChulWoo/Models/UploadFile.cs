using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChulWoo.Models
{
    public class UploadFile
    {
        public int ID { get; set; }

        [StringLength(256, MinimumLength = 3, ErrorMessage = "Want 3 ~ 256 characters.")]
        public string FileName { get; set; }

        [StringLength(256, MinimumLength = 3, ErrorMessage = "Want 3 ~ 256 characters.")]
        public string SaveFileName { get; set; }

        [StringLength(256, MinimumLength = 3, ErrorMessage = "Want 3 ~ 256 characters.")]
        public string FolderName { get; set; }
    }
}