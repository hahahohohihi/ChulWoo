using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChulWoo.Models;

namespace ChulWoo.Viewmodel
{
    public class HomeInfoData
    {
        public virtual ICollection<Personnel> Personnels { get; set; }

        public virtual ICollection<WorkUnit> WorkUnits { get; set; }

    }
}