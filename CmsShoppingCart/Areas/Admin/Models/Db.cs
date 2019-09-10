using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CmsShoppingCart.Areas.Admin.Models.Data;

namespace CmsShoppingCart.Areas.Admin.Models
{
    public class Db: DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }
    }
}