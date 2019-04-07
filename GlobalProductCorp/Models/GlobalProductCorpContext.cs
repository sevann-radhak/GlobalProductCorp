using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace GlobalProductCorp.Models
{
    public class GlobalProductCorpContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public GlobalProductCorpContext() : base("name=GlobalProductCorpContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<GlobalProductCorp.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<GlobalProductCorp.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<GlobalProductCorp.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<GlobalProductCorp.Models.OrderDetail> OrderDetails { get; set; }
    }
}
