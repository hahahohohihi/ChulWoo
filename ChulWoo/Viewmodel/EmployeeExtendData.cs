using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChulWoo.Models;

namespace ChulWoo.Viewmodel
{
    public class EmployeeExtendData
    {
        public Employee Employee { get; set; }
        public Contract Contract { get; set; }
        public Personnel Personnel { get; set; }
    }
}