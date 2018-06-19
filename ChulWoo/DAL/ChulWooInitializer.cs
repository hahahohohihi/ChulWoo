using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ChulWoo.Models;
using System.Data.Entity.Migrations; 

namespace ChulWoo.DAL
{
    public class ChulWooInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ChulWooContext>
    {
        protected override void Seed(ChulWooContext context)
        {
        }
    }
}